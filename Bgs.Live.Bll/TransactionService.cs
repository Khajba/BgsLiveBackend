using Bgs.Live.Bll.Abstract;
using Bgs.Live.Common.Dtos;
using Bgs.Live.Dal.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Bll
{
    public class TransactionService:ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public IEnumerable<TransactionDto> GetTransactions(int? typeId, string pinCode, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, int? pageNumber, int? pageSize)
        {
            return _transactionRepository.GetTransactions(null, typeId, pinCode, dateFrom, dateTo, amountFrom, amountTo, pageNumber, pageSize);
        }

        public int GetTransactionsCount(int? typeId, string pinCode, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo)
        {
            return _transactionRepository.GetTransactionsCount(typeId, pinCode, dateFrom, dateTo, amountFrom, amountTo);
        }

        
    }
}
