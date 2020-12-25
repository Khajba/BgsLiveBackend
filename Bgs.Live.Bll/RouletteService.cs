using Bgs.Bll.Abstract;
using Bgs.DataConnectionManager.SqlServer.SqlClient;
using Bgs.Live.Bll.Abstract;
using Bgs.Live.Common.Enums;
using Bgs.Live.Common.ErrorCodes;
using Bgs.Live.Common.Models;
using Bgs.Live.Core.Exceptions;
using Bgs.Live.Dal.Abstract;
using System;
using System.Threading.Tasks;

namespace Bgs.Live.Bll
{
    public class RouletteService : IRouletteService
    {
        private readonly IUserRepository _UserRepository;
        private readonly ITransactionRepository _TransactionRepository;
        
        public RouletteService(IUserRepository userRepository, ITransactionRepository transactionRepository)
        {
            _UserRepository = userRepository;
            _TransactionRepository = transactionRepository;
        }

        public async Task<BetResponseModel> Bet(int userId, decimal amount, decimal number)
        {
            var balance = await _UserRepository.GetBalance(userId);
            balance = balance - amount;

            if (balance < 0)
            {
                await _TransactionRepository.AddTransaction((int)TransactionType.Bet, userId, (int)TransactionStatus.Failed, DateTime.Now, amount);
                throw new BgsException((int)WebApiErrorCodes.NotEnoughBalance);
            }

            var winNumber = new Random().Next(0, 36);
            decimal winAmount = 0;

            if (winNumber == number)
            {
                winAmount = amount * 36;
                balance = balance + winAmount;
            }

            using (var transaction = new BgsTransactionScope())
            {
                await _UserRepository.UpdateBalance(userId, balance);
                await _TransactionRepository.AddTransaction((int)TransactionType.Win, userId, (int)TransactionStatus.Success, DateTime.Now, winAmount);

                transaction.Complete();
            }

            return new BetResponseModel
            {
                WinAmount = winAmount,
                WinnerNumber = winNumber
            };
        }
    }
}
