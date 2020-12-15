using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Bgs.DataConnectionManager.SqlServer.SqlClient
{
    public class BgsSqlConnection : IDbConnection, IDisposable
    {
        #region Properties

        protected SqlConnection Connection { get; set; }

        #endregion

        #region Constructors

        public BgsSqlConnection(SqlConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException("connection");
        }

        public BgsSqlConnection(string connectionString)
            : this(new SqlConnection(connectionString))
        {

        }

        #endregion

        public SqlTransaction BeginTransaction(IsolationLevel il)
        {
            return Connection.BeginTransaction(il);
        }

        public SqlTransaction BeginTransaction()
        {
            return Connection.BeginTransaction();
        }

        public BgsSqlCommand CreateCommand()
        {
            return new BgsSqlCommand(Connection.CreateCommand());
        }

        #region Implementations

        #region IDbConnection

        public ConnectionState State => Connection.State;
        public string Database => Connection.Database;
        public int ConnectionTimeout => Connection.ConnectionTimeout;

        IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il)
        {
            return BeginTransaction(il);
        }

        IDbTransaction IDbConnection.BeginTransaction()
        {
            return BeginTransaction();
        }

        public void ChangeDatabase(string databaseName)
        {
            Connection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            Connection.Close();
        }

        public string ConnectionString
        {
            get => Connection.ConnectionString;
            set
            {
                //Connection.ConnectionString = value;
            }
        }

        IDbCommand IDbConnection.CreateCommand() { return CreateCommand(); }

        public void Open()
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
        }

        public async Task OpenAsync(CancellationToken cancellationToken = default)
        {
            if (Connection.State == ConnectionState.Closed)
                await Connection.OpenAsync(cancellationToken);
        }

        #endregion

        #region IDispose

        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
        }

        #endregion

        #endregion
    }
}