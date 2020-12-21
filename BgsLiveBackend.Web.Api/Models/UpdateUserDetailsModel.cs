using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BgsLiveBackend.Web.Api.Models
{
    public class UpdateUserDetailsModel
    {
       public string Firstname { get; set; }
       public string Lastname { get; set; }
       public DateTime BirthDate { get; set; }
       public int GenderId { get; set; }
       public string Address { get; set; }
       public string PhoneNumber { get; set; }
    }
}
