using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Bgs.DataConnectionManager.SqlServer.SqlClient
{
    public class BgsSqlCommand : IDbCommand, IDisposable
    {
        protected SqlCommand Command { get; set; }

        public SqlConnection Connection
        {
            get => Command.Connection;
            set => Command.Connection = value;
        }

        public SqlTransaction Transaction
        {
            get => Command.Transaction;
            set => Command.Transaction = value;
        }

        public BgsSqlCommand(SqlCommand command)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public void AddParameter(string name, object value)
        {
            var parameter = Command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;

            Command.Parameters.Add(parameter);
        }

        public void AddParameter(string name, object value, SqlDbType type)
        {
            var parameter = Command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            parameter.SqlDbType = type;

            Command.Parameters.Add(parameter);
        }

        public IDataParameter AddOutParameter(string name, DbType dbType, int size = 0)
        {
            var parameter = Command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Direction = ParameterDirection.Output;
            parameter.DbType = dbType;
            parameter.Size = size;

            Command.Parameters.Add(parameter);

            return parameter;
        }

        public IDataReader ExecuteReader(CommandBehavior behaviour)
        {
            return Command.ExecuteReader(behaviour);
        }

        public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken = default)
        {
            return await Command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<IDataReader> ExecuteReaderAsync(CancellationToken cancellationToken = default)
        {
            return await Command.ExecuteReaderAsync(cancellationToken);
        }

        protected T CloseConnectionAfter<T>(Func<T> function)
        {
            if (Transaction != null)
            {
                return function();
            }

            try
            {
                return function();
            }
            catch (Exception ex)
            {
                if (Command.Connection.State == ConnectionState.Open)
                {
                    Command.Connection.Close();
                }

                throw new Exception(ex.Message, ex);
            }
        }

        public void Close()
        {
            if (Command == null)
                return;

            if (Transaction == null)
            {
                Command.Connection.Close();
            }

            Command.Dispose();
        }

        #region Implementations

        #region IDbCommand

        public IDataParameterCollection Parameters => Command.Parameters;

        public void Cancel()
        {
            Command.Cancel();
        }

        public string CommandText
        {
            get => Command.CommandText;
            set => Command.CommandText = value;
        }

        public int CommandTimeout
        {
            get => Command.CommandTimeout;
            set => Command.CommandTimeout = value;
        }

        public CommandType CommandType
        {
            get => Command.CommandType;
            set => Command.CommandType = value;
        }

        IDbConnection IDbCommand.Connection
        {
            get => Connection;
            set
            {
                if (value is SqlConnection connection)
                {
                    Connection = connection;
                }
                else
                {
                    throw new ArgumentException("Connection type should be SqlConnection");
                }
            }
        }

        public IDbDataParameter CreateParameter()
        {
            return Command.CreateParameter();
        }

        public int ExecuteNonQuery()
        {
            return CloseConnectionAfter(() => Command.ExecuteNonQuery());
        }

        public IDataReader ExecuteReader()
        {
            return CloseConnectionAfter(() => Command.ExecuteReader());
        }

        public object ExecuteScalar()
        {
            return CloseConnectionAfter(() => Command.ExecuteScalar());
        }

        public T ExecuteScalar<T>()
        {
            return CloseConnectionAfter(() => (T)Command.ExecuteScalar());
        }

        public async Task<T> ExecuteScalarAsync<T>(CancellationToken cancellationToken = default)
        {
            return await CloseConnectionAfter(async () =>
            {
                var value = await Command.ExecuteScalarAsync(cancellationToken);

                return (T)(value == DBNull.Value ? null : value);
            });
        }

        public async Task<object> ExecuteScalarAsync()
        {
            return await CloseConnectionAfter(async () => await Command.ExecuteScalarAsync());
        }

        public void Prepare()
        {
            Command.Prepare();
        }

        IDbTransaction IDbCommand.Transaction
        {
            get => Transaction;
            set
            {
                if (value is SqlTransaction transaction)
                {
                    Transaction = transaction;
                }
                else
                {
                    throw new ArgumentException("Transaction type should be SqlTransaction");
                }
            }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get => Command.UpdatedRowSource;
            set => Command.UpdatedRowSource = value;
        }

        #endregion

        #region IDispose

        public void Dispose()
        {
            if (Command == null)
                return;

            if (Transaction == null && System.Transactions.Transaction.Current == null)
            {
                Command.Connection.Dispose();
            }

            Command.Dispose();
            Command = null;
        }

        #endregion

        #endregion
    }
}