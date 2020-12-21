using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Common.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string PinCode { get; set; }

        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Username { get; set; }

        public string Lastname { get; set; }        

        public int GenderId { get; set; }        

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime BirthDate { get; set; }

        public int StatusId { get; set; }

        
    }
}
