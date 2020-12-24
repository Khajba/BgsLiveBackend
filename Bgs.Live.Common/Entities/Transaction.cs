using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Common.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }        
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
