using Bgs.Live.Bll.Abstract;
using Bgs.Live.Common.Dtos;
using Bgs.Live.Dal.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Bll
{
    public class TransactionService:ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<IEnumerable<TransactionDto>> GetTransactions(int? typeId, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, int? pageNumber, int? pageSize)
        {
            return await  _transactionRepository.GetTransactions(null, typeId, dateFrom, dateTo, amountFrom, amountTo, pageNumber, pageSize);
        }

        public async Task<int> GetTransactionsCount(int? typeId, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo)
        {
            return await _transactionRepository.GetTransactionsCount(typeId,dateFrom, dateTo, amountFrom, amountTo);
        }

        
    }
}
