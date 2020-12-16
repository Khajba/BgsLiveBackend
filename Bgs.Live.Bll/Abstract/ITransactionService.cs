using Bgs.Live.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Bll.Abstract
{
    public interface ITransactionService
    {
        IEnumerable<TransactionDto> GetTransactions(int? typeId, string pinCode, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, int? pageNumber, int? pageSize);

        public int GetTransactionsCount(int? typeId, string pinCode, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo);
    }
}
