using Bgs.Live.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Dal.Abstract
{
    public interface ITransactionRepository
    {
        public Task AddTransaction(int typeId, int userId, DateTime createDate, decimal amount);

        public Task<IEnumerable<TransactionDto>> GetTransactions(int? userId, int? typeId, string pincode, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, int? pageNumber, int? pageSize);

        public Task<int> GetTransactionsCount(int? typeId, string pinCode, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo);


    }
}
