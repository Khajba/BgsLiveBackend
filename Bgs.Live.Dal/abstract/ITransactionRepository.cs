using Bgs.Live.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Dal.Abstract
{
    public interface ITransactionRepository
    {
        public void AddTransaction(int typeId, int userId, DateTime createDate, decimal amount);

        public IEnumerable<TransactionDto> GetTransactions(int? userId, int? typeId, string pincode, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, int? pageNumber, int? pageSize);

        public int GetTransactionsCount(int? typeId, string pinCode, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo);


    }
}
