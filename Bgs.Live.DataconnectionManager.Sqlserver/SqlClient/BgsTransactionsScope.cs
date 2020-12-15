using System;
using System.Runtime.Caching;
using System.Transactions;

namespace Bgs.DataConnectionManager.SqlServer.SqlClient
{
    public class BgsTransactionScope : IDisposable
    {
        private readonly bool _hasParent;

        private Transaction _transaction;
        private TransactionScope _transactionScope;

        private static readonly TimeSpan Timeout = new TimeSpan(0, 2, 30);

        public BgsTransactionScope()
            : this(TransactionScopeOption.Required)
        {

        }

        public BgsTransactionScope(TransactionScopeOption option)
        {
            _hasParent = (Transaction.Current != null && option != TransactionScopeOption.RequiresNew) || option == TransactionScopeOption.Suppress;
            _transactionScope = new TransactionScope(option, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = Timeout }, TransactionScopeAsyncFlowOption.Enabled);
            _transaction = Transaction.Current;
        }

        public void Complete()
        {
            _transactionScope.Complete();
        }

        public void Dispose()
        {
            if (!_hasParent)
            {
                DisposeConnection(_transaction);
            }

            _transactionScope.Dispose();

            _transaction = null;
            _transactionScope = null;
        }

        private void DisposeConnection(Transaction transaction)
        {
            if (MemoryCache.Default[transaction.TransactionInformation.LocalIdentifier] is BgsSqlConnection connection)
            {
                connection.Dispose();
            }
        }
    }
}