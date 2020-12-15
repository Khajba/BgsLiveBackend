using Bgs.DataConnectionManager.SqlServer.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bgs.DataConnectionManager.SqlServer.Extensions
{
    public static class AdoExtensions
    {
        #region IDbCommand

        public static void AddParameters(this IDbCommand command, IEnumerable<SqlParameter> parameters)
        {
            foreach (var param in parameters)
            {
                command.Parameters.Add(param);
            }
        }

        public static bool ExistsAnyData(this IDbCommand command)
        {
            try
            {
                using var reader = command.ExecuteReader();
                {
                    return reader.Read();
                }
            }
            finally
            {
                if (command.Transaction == null)
                {
                    command.Connection.Dispose();
                    command.Dispose();
                }
            }
        }

        public static bool ExecuteReaderAnyClosed(this IDbCommand command)
        {
            try
            {
                using var reader = command.ExecuteReader();
                {
                    return reader.Read();
                }
            }
            finally
            {
                if (command.Transaction == null)
                {
                    command.Connection.Dispose();
                    command.Dispose();
                }
            }
        }

        public static IEnumerable<T> ExecuteReaderClosed<T>(this IDbCommand command, Action<T, IDataReader> callback = null)
            where T : class, new()
        {
            try
            {
                using var reader = command.ExecuteReader();
                {
                    return ExecuteReaderInternal(reader, callback);
                }
            }
            finally
            {
                if (command.Transaction == null)
                {
                    command.Connection.Dispose();
                    command.Dispose();
                }
            }
        }

        public static IEnumerable<T> ExecuteReader<T>(this IDbCommand command, Action<T, IDataReader> callback = null)
            where T : class, new()
        {
            using var reader = command.ExecuteReader();
            {
                return ExecuteReaderInternal(reader, callback);
            }
        }

        public static async Task<IEnumerable<T>> ExecuteReaderClosedAsync<T>(this IDbCommand command, Action<T, IDataReader> callback = null, CancellationToken cancellationToken = default)
           where T : class, new()
        {
            try
            {
                using var reader = await ((BgsSqlCommand)command).ExecuteReaderAsync(cancellationToken);
                {
                    return ExecuteReaderInternal<T>(reader, callback);
                }
            }
            finally
            {
                if (command.Transaction == null)
                {
                    command.Connection.Dispose();
                    command.Dispose();
                }
            }
        }

        public static async Task<T> ExecuteReaderSingleAsync<T>(this IDbCommand command, Action<T, IDataReader> callback = null, CancellationToken cancellationToken = default)
        where T : class, new()
        {
            using var reader = await ((BgsSqlCommand)command).ExecuteReaderAsync(cancellationToken);
            {
                return ExecuteReaderInternal<T>(reader, callback).FirstOrDefault();
            }
        }

        public static async Task<IEnumerable<T>> ExecuteReaderAsync<T>(this IDbCommand command, Action<T, IDataReader> callback = null, CancellationToken cancellationToken = default)
         where T : class, new()
        {
            using var reader = await ((BgsSqlCommand)command).ExecuteReaderAsync(cancellationToken);
            {
                return ExecuteReaderInternal<T>(reader, callback);
            }
        }

        public static T ExecuteReaderSingleClosed<T>(this IDbCommand command)
            where T : class, new()
        {
            return command.ExecuteReaderClosed<T>().FirstOrDefault();
        }

        public static T ExecuteReaderSingle<T>(this IDbCommand command)
            where T : class, new()
        {
            return command.ExecuteReader<T>().FirstOrDefault();
        }

        public static T ExecuteReaderSingleClosed<T>(this IDbCommand command, Action<T, IDataReader> callback)
           where T : class, new()
        {
            return command.ExecuteReaderClosed(callback).FirstOrDefault();
        }

        public static T ExecuteReaderPrimitive<T>(this IDbCommand command, string name)
        {
            using var reader = command.ExecuteReader();
            {
                var isPresented = reader.Read();

                if (!isPresented)
                    return default;

                var val = reader[name].ToString();

                return ParseEntry<T>(val);
            }
        }

        public static T ExecuteReaderPrimitiveClosed<T>(this IDbCommand command, string name)
        {
            try
            {
                using var reader = command.ExecuteReader();
                {
                    var isPresented = reader.Read();

                    if (!isPresented)
                        return default;

                    var val = reader[name].ToString();

                    return ParseEntry<T>(val);
                }
            }
            finally
            {
                if (command.Transaction == null)
                {
                    command.Connection.Dispose();
                    command.Dispose();
                }
            }
        }

        public static async Task<T> ExecuteReaderPrimitiveAsync<T>(this IDbCommand command, string name, CancellationToken cancellationToken = default)
        {
            using var reader = (SqlDataReader)await ((BgsSqlCommand)command).ExecuteReaderAsync(cancellationToken);
            {
                var isPresented = await reader.ReadAsync(cancellationToken);

                if (!isPresented)
                    return default;

                var value = reader[name].ToString();

                return ParseEntry<T>(value);
            }
        }

        public static async Task<T> ExecuteReaderPrimitiveClosedAsync<T>(this IDbCommand command, string name, CancellationToken cancellationToken = default)
        {
            try
            {
                using var reader = (SqlDataReader)await ((BgsSqlCommand)command).ExecuteReaderAsync(cancellationToken);
                {
                    var isPresented = await reader.ReadAsync(cancellationToken);

                    if (!isPresented)
                        return default;

                    var value = reader[name].ToString();

                    return ParseEntry<T>(value);
                }
            }
            finally
            {
                if (command.Transaction == null)
                {
                    command.Connection.Dispose();
                    command.Dispose();
                }
            }
        }

        public static IEnumerable<T> ExecuteReaderPrimitives<T>(this IDbCommand command, string name)
        {
            using var reader = (SqlDataReader)((BgsSqlCommand)command).ExecuteReader();
            {
                var result = new List<T>();
                while (reader.Read())
                {
                    var val = reader[name].ToString();

                    result.Add(ParseEntry<T>(val));
                }

                return result;
            }
        }

        public static async Task<IEnumerable<T>> ExecuteReaderPrimitivesAsync<T>(this IDbCommand command, string name, CancellationToken cancellationToken = default)
        {
            using var reader = (SqlDataReader)await ((BgsSqlCommand)command).ExecuteReaderAsync(cancellationToken);
            {
                var result = new List<T>();
                while (await reader.ReadAsync(cancellationToken))
                {
                    var val = reader[name].ToString();

                    result.Add(ParseEntry<T>(val));
                }

                return result;
            }
        }

        public static async Task<IEnumerable<T>> ExecuteReaderPrimitivesClosedAsync<T>(this IDbCommand command, string name, CancellationToken cancellationToken = default)
        {
            try
            {
                using var reader = (SqlDataReader)await ((BgsSqlCommand)command).ExecuteReaderAsync(cancellationToken);
                {
                    var result = new List<T>();
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var val = reader[name].ToString();

                        result.Add(ParseEntry<T>(val));
                    }

                    return result;
                }
            }
            finally
            {
                if (command.Transaction == null)
                {
                    command.Connection.Dispose();
                    command.Dispose();
                }
            }
        }

        public static T ExecuteReaderInsertClosed<T>(this IDbCommand command, string keyName = "Id")
            where T : struct
        {
            try
            {
                using var reader = command.ExecuteReader();
                {
                    while (reader.Read())
                    {
                        return ParseEntry<T>(reader[keyName].ToString());
                    }
                }
            }
            finally
            {
                if (command.Transaction == null)
                {
                    command.Connection.Dispose();
                    command.Dispose();
                }
            }

            return default;
        }

        public static T ExecuteReaderSingleValue<T>(this IDbCommand command, string keyName)
            where T : struct
        {
            try
            {
                using var reader = command.ExecuteReader();
                {
                    while (reader.Read())
                    {
                        return ParseEntry<T>(keyName);
                    }
                }
            }
            finally
            {
                if (command.Transaction == null)
                {
                    command.Connection.Dispose();
                    command.Dispose();
                }
            }

            return default;
        }

        public static void ExecuteNonQueryClosed(this IDbCommand command)
        {
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                if (command.Transaction == null)
                {
                    command.Connection.Dispose();
                    command.Dispose();
                }
            }
        }

        #endregion

        #region DbCommand

        public static async Task<IEnumerable<T>> ExecuteReaderClosedAsync<T>(this DbCommand command, CancellationToken cancellationToken = default)
            where T : class
        {
            var entities = new List<T>();

            var props = typeof(T).GetProperties();

            using (command)
            {
                using (command.Connection)
                {
                    using var reader = await command.ExecuteReaderAsync(cancellationToken);
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            var entity = Activator.CreateInstance<T>();

                            for (var index = 0; index < reader.FieldCount; index++)
                            {
                                var fieldName = reader.GetName(index);
                                var fieldValue = reader[index];

                                if (fieldValue != DBNull.Value && fieldValue != null)
                                {
                                    try
                                    {
                                        var propInfo = props.FirstOrDefault(p => p.Name.EqualsIgnoreCase(fieldName));

                                        if (propInfo == null)
                                            continue;

                                        if (fieldValue is decimal &&
                                            (propInfo.PropertyType == typeof(double?) ||
                                             propInfo.PropertyType == typeof(double)))
                                        {
                                            var propValue = (double)decimal.Parse(fieldValue.ToString());
                                            propInfo.SetValue(entity, propValue);
                                        }
                                        else if (fieldValue is DateTime &&
                                                 (propInfo.PropertyType == typeof(long?) ||
                                                  propInfo.PropertyType == typeof(long)))
                                        {
                                            var propValue = DateTime.Parse(fieldValue.ToString()).Ticks;
                                            propInfo.SetValue(entity, propValue);
                                        }
                                        else if (fieldValue is long &&
                                                 (propInfo.PropertyType == typeof(int?) ||
                                                  propInfo.PropertyType == typeof(int)))
                                        {
                                            var propValue = int.Parse(fieldValue.ToString());
                                            propInfo.SetValue(entity, propValue);
                                        }
                                        else if (fieldValue is int &&
                                                 Nullable.GetUnderlyingType(propInfo.PropertyType) != null &&
                                                 Nullable.GetUnderlyingType(propInfo.PropertyType).IsEnum)
                                        {
                                            var propValue = Enum.ToObject(Nullable.GetUnderlyingType(propInfo.PropertyType), int.Parse(fieldValue.ToString()));
                                            propInfo.SetValue(entity, propValue);
                                        }
                                        else
                                        {
                                            propInfo.SetValue(entity, fieldValue);
                                        }

                                    }
                                    catch (Exception)
                                    {
                                        //Log error
                                    }
                                }
                            }

                            entities.Add(entity);
                        }
                    }
                }
            }

            return entities;
        }

        #endregion

        #region SqlCommand

        public static void AddParameter(this SqlCommand command, string name, object value)
        {
            command.Parameters.AddWithValue(name, value);
        }

        public static async Task ExecuteNonQueryClosedAsync(this SqlCommand command, CancellationToken cancellationToken = default)
        {
            using (command)
            {
                using (command.Connection)
                {
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        public static async Task<T> ExecuteReaderSingleClosedAsync<T>(this SqlCommand command, CancellationToken cancellationToken = default)
            where T : class
        {
            return (await command.ExecuteReaderClosedAsync<T>(cancellationToken)).FirstOrDefault();
        }

        #endregion

        #region BgsSqlCommand

        public static async Task ExecuteNonQueryClosedAsync(this BgsSqlCommand command, CancellationToken cancellationToken = default)
        {
            using (command)
            {
                using (command.Connection)
                {
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        public static async Task<T> ExecuteReaderSingleClosedAsync<T>(this BgsSqlCommand command, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            return (await command.ExecuteReaderClosedAsync<T>(cancellationToken: cancellationToken)).FirstOrDefault();
        }

        #endregion

        #region String

        public static bool EqualsIgnoreCase(this string text, string comparing)
        {
            return string.Compare(text, comparing, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase) == 0;
        }

        #endregion

        #region Helper methods

        private static T ParseEntry<T>(string entry)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            return (T)converter.ConvertFromString(entry);
        }

        private static IEnumerable<T> ExecuteReaderInternal<T>(IDataReader reader, Action<T, IDataReader> callback = null)
            where T : class, new()
        {
            var entities = new List<T>();

            var props = typeof(T).GetProperties();

            while (reader.Read())
            {
                var entity = Activator.CreateInstance<T>();

                for (var index = 0; index < reader.FieldCount; index++)
                {
                    var fieldName = reader.GetName(index);
                    var fieldValue = reader[index];

                    if (fieldValue != DBNull.Value && fieldValue != null)
                    {
                        try
                        {
                            var propInfo = props.FirstOrDefault(p => p.Name.EqualsIgnoreCase(fieldName));

                            if (propInfo == null)
                                continue;

                            if (fieldValue is decimal &&
                                (propInfo.PropertyType == typeof(double?) ||
                                 propInfo.PropertyType == typeof(double)))
                            {
                                var propValue = (double)decimal.Parse(fieldValue.ToString());
                                propInfo.SetValue(entity, propValue);
                            }
                            else if (fieldValue is DateTime &&
                                     (propInfo.PropertyType == typeof(long?) ||
                                      propInfo.PropertyType == typeof(long)))
                            {
                                var propValue = DateTime.Parse(fieldValue.ToString()).Ticks;
                                propInfo.SetValue(entity, propValue);
                            }
                            else if (fieldValue is long &&
                                     (propInfo.PropertyType == typeof(int?) ||
                                      propInfo.PropertyType == typeof(int)))
                            {
                                var propValue = int.Parse(fieldValue.ToString());
                                propInfo.SetValue(entity, propValue);
                            }
                            else if (fieldValue is int &&
                                     Nullable.GetUnderlyingType(propInfo.PropertyType) != null &&
                                     Nullable.GetUnderlyingType(propInfo.PropertyType).IsEnum)
                            {
                                var propValue = Enum.ToObject(Nullable.GetUnderlyingType(propInfo.PropertyType), int.Parse(fieldValue.ToString()));
                                propInfo.SetValue(entity, propValue);
                            }
                            else if (fieldValue is short && propInfo.PropertyType == typeof(int))
                            {
                                var val = int.Parse(fieldValue.ToString());
                                propInfo.SetValue(entity, val);
                            }
                            else
                            {
                                propInfo.SetValue(entity, fieldValue);
                            }
                        }
                        catch (Exception ex)
                        {
                            //Log error
                        }
                    }
                }

                callback?.Invoke(entity, reader);

                entities.Add(entity);
            }

            return entities;
        }

        #endregion
    }
}