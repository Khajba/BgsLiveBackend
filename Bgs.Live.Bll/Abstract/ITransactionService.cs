using Bgs.Live.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Bll.Abstract
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactions(int? typeId, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, int? pageNumber, int? pageSize);

        public Task<int> GetTransactionsCount(int? typeId, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo);
    }
}
