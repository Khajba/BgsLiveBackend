using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Common.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public string Type { get; set; }

        public DateTime CreateDate { get; set; }

        public string PinCode { get; set; }

        public decimal Amount { get; set; }
    }
}
