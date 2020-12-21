using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Common.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PinCode { get; set; }
        public string Firstname { get; set; }
        public string Username { get; set; }
        public string Lastname { get; set; }
        public string GenderId { get; set; }
        public int StatusId { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNUmber { get; set; }        
        public string AvatarUrl { get; set; }
    }
}
