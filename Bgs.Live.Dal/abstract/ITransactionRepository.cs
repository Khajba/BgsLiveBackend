using Bgs.Live.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Dal.Abstract
{
    public interface ITransactionRepository
    {
        public Task AddTransaction(int typeId, int userId, int? statusId, DateTime createDate, decimal amount);

        public Task<IEnumerable<TransactionDto>> GetTransactions(int? userId, int? typeId,DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, int? pageNumber, int? pageSize);

        public Task<int> GetTransactionsCount(int? typeId, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo);


    }
}
