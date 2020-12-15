using Bgs.DataConnectionManager.SqlServer.Abstract;
using Bgs.DataConnectionManager.SqlServer.Extensions;
using Bgs.DataConnectionManager.SqlServer.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Bgs.DataConnectionManager.SqlServer
{
    public class SqlServerRepository : DalBase<BgsSqlCommand, BgsSqlConnection>
    {
        #region Private  Fields

        private string ConnectionString { get; set; }

        private static readonly object LockObject = new object();

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        public SqlServerRepository(IConfiguration configuration, string connectionString)
        {
            ConnectionString = connectionString;
            _configuration = configuration;
        }

        #endregion

        #region Protected Methods

        /// <summary>Open new SqlConnection</summary>
        protected override BgsSqlConnection GetConnection(bool fromArchive)
        {
            lock (LockObject)
            {
                var dbConnection = GetConnectionInternal(fromArchive);

                dbConnection.Open();

                return dbConnection;
            }
        }

        private BgsSqlConnection GetConnectionInternal(bool fromArchive = false)
        {
            var dbConnection = null as BgsSqlConnection;
            var currentTransaction = Transaction.Current;

            if (currentTransaction != null)
            {
                Monitor.Enter(currentTransaction);
            }

            try
            {
                if (Transaction.Current != null)
                {
                    dbConnection = MemoryCache.Default[Transaction.Current.TransactionInformation.LocalIdentifier] as BgsSqlConnection;
                }

                if (dbConnection == null)
                {
                    var connString = GetConnectionString(fromArchive);
                    dbConnection = new BgsSqlConnection(connString);

                    if (Transaction.Current != null)
                    {
                        MemoryCache.Default.Add(Transaction.Current.TransactionInformation.LocalIdentifier, dbConnection, DateTime.UtcNow.AddSeconds(120));
                    }
                }
            }
            finally
            {
                if (currentTransaction != null)
                    Monitor.Exit(currentTransaction);
            }

            return dbConnection;
        }

        private string GetConnectionString(bool fromArchive)
        {
            var connString = ConnectionString;

            if (!fromArchive)
                return connString;

            var archiveConnection = _configuration.GetConnectionString("ArchiveDB");

            if (!string.IsNullOrEmpty(archiveConnection))
                connString = archiveConnection;

            return connString;
        }

        protected override async Task<BgsSqlConnection> GetConnectionAsync(bool fromArchive, CancellationToken cancellationToken = default)
        {
            var dbConnection = GetConnectionInternal(fromArchive);

            switch (dbConnection.State)
            {
                case ConnectionState.Closed:
                    await dbConnection.OpenAsync(cancellationToken);
                    break;
                case ConnectionState.Connecting:
                    await Task.Run(() =>
                    {
                        while (dbConnection.State == ConnectionState.Connecting)
                        {
                            Thread.Sleep(10);
                        }
                    });
                    break;
                case ConnectionState.Open:
                    break;
                case ConnectionState.Executing:
                    break;
                case ConnectionState.Fetching:
                    break;
                case ConnectionState.Broken:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return dbConnection;
        }

        public static T MapReaderSingle<T>(IDataReader reader, Action<T, IDataReader> callback = null, bool read = true) where T : class, new()
        {
            if (read)
                return MapReader(reader, callback).FirstOrDefault();

            var entities = new List<T>();

            var entity = MapEntity(reader, entities);

            callback?.Invoke(entity, reader);

            return entity;
        }

        private static IEnumerable<T> MapReader<T>(IDataReader reader, Action<T, IDataReader> callback = null, bool readFirst = true)
            where T : class, new()
        {
            var entities = new List<T>();

            do
            {
                var read = true;

                if (readFirst)
                    read = reader.Read();

                if (read)
                {
                    var entity = MapEntity(reader, entities);

                    callback?.Invoke(entity, reader);
                }

                readFirst = false;
            }
            while (reader.Read());

            return entities;
        }

        private static T MapEntity<T>(IDataReader reader, List<T> entities) where T : class, new()
        {
            var props = typeof(T).GetProperties();
            var entity = new T();

            entities.Add(entity);

            for (int index = 0; index < reader.FieldCount; index++)
            {
                var fieldName = reader.GetName(index);
                var fieldValue = reader[index];

                try
                {
                    if (fieldValue != DBNull.Value && fieldValue != null)
                    {
                        var propInfo = props.FirstOrDefault(p => p.Name.EqualsIgnoreCase(fieldName));

                        if (propInfo == null)
                            continue;

                        if (fieldValue is decimal &&
                            (propInfo.PropertyType == typeof(double?) ||
                             propInfo.PropertyType == typeof(double)) ||
                            fieldValue is double)
                        {
                            object val;

                            if (propInfo.PropertyType == typeof(float))
                            {
                                val = (float)decimal.Parse(fieldValue.ToString());
                            }
                            else
                            {
                                val = (double)decimal.Parse(fieldValue.ToString());
                            }

                            propInfo.SetValue(entity, val);
                        }
                        else if (fieldValue is DateTime &&
                                 (propInfo.PropertyType == typeof(long?) ||
                                  propInfo.PropertyType == typeof(long)))
                        {
                            var val = DateTime.Parse(fieldValue.ToString()).Ticks;
                            propInfo.SetValue(entity, val);
                        }
                        else if (fieldValue is long &&
                                 (propInfo.PropertyType == typeof(int?) ||
                                  propInfo.PropertyType == typeof(int)))
                        {
                            var val = int.Parse(fieldValue.ToString());
                            propInfo.SetValue(entity, val);
                        }
                        else if (fieldValue is int &&
                                 Nullable.GetUnderlyingType(propInfo.PropertyType) != null &&
                                 Nullable.GetUnderlyingType(propInfo.PropertyType).IsEnum)
                        {
                            var val = Enum.ToObject(Nullable.GetUnderlyingType(propInfo.PropertyType), int.Parse(fieldValue.ToString()));
                            propInfo.SetValue(entity, val);
                        }
                        else
                        {
                            propInfo.SetValue(entity, fieldValue);
                        }
                    }
                }
                catch (Exception)
                {
                    //Log error
                }
            }

            return entity;
        }

        #endregion
    }
}