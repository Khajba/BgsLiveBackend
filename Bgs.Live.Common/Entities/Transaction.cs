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
        public decimal amount { get; set; }

    }
}
