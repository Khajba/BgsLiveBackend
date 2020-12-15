using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BgsLiveBackend.Admin.Api.Models.Account
{
    public class AuthenticateUserModel
    {
        [Required(ErrorMessage = "required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "required")]
        public string Password { get; set; }
    }
}
