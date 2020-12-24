using System;

namespace Bgs.Live.Common.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }

        public int StatusId { get; set; }

        public int TypeId { get; set; }        

        public DateTime CreateDate { get; set; }        

        public decimal Amount { get; set; }

        public decimal Fee { get; set; }
    }
}
