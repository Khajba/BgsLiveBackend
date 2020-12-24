using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BgsLiveBackend.Web.Api.Models
{
    public class TransactionFilterModel
    {
        public int TypeId { get; set; }
        public string PinCode { get; set; }
        public decimal Amountfrom { get; set; }
        public decimal AmountTo { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }        
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        
    }
}
