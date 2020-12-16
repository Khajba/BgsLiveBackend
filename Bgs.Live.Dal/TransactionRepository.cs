using Bgs.DataConnectionManager.SqlServer;
using Bgs.DataConnectionManager.SqlServer.Extensions;
using Bgs.Live.Common.Dtos;
using Bgs.Live.Dal.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Dal
{
    public class TransactionRepository : SqlServerRepository, ITransactionRepository
    {

        private const string _schemaTransaction = "Transaction";

        public TransactionRepository(IConfiguration configuration)
             : base(configuration, configuration.GetConnectionString("MainDatabase"))
        {

        }
        public void AddTransaction(int typeId, int userId, DateTime createDate, decimal amount)
        {
            using (var cmd = GetSpCommand($"{_schemaTransaction}.AddTransaction"))
            {
                cmd.AddParameter("TypeId", typeId);
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("CreateDate", createDate);
                cmd.AddParameter("Amount", amount);

                cmd.ExecuteNonQuery();
            };
        }

        public IEnumerable<TransactionDto> GetTransactions(int? userId, int? typeId, string pincode, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo, int? pageNumber, int? pageSize)
        {
            using (var cmd = GetSpCommand($"{_schemaTransaction}.GetTransactions"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("TypeId", typeId);
                cmd.AddParameter("Pincode", pincode);
                cmd.AddParameter("StartDate", dateFrom);
                cmd.AddParameter("EndDate", dateTo);
                cmd.AddParameter("AmountFrom", amountFrom);
                cmd.AddParameter("AmountTo", amountTo);
                cmd.AddParameter("PageNumber", pageNumber);
                cmd.AddParameter("PageSize", pageSize);


                return cmd.ExecuteReader<TransactionDto>();
            }
        }

        public int GetTransactionsCount(int? typeId, string pinCode, DateTime? dateFrom, DateTime? dateTo, decimal? amountFrom, decimal? amountTo)
        {
            using (var cmd = GetSpCommand($"{_schemaTransaction}.GetTransactionsCount"))
            {
                cmd.AddParameter("TypeId", typeId);
                cmd.AddParameter("Pincode", pinCode);
                cmd.AddParameter("StartDate", dateFrom);
                cmd.AddParameter("EndDate", dateTo);
                cmd.AddParameter("AmountFrom", amountFrom);
                cmd.AddParameter("AmountTo", amountTo);

                return cmd.ExecuteReaderPrimitive<int>("Count");
            };
        }
    }
}
