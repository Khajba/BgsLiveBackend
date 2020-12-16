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

        public decimal Balance { get; set; }

        public string AvatarUrl { get; set; }
    }
}
