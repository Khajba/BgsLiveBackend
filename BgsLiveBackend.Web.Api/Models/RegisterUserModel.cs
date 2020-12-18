using System;
using System.ComponentModel.DataAnnotations;

namespace BgsLiveBackend.Web.Api.Models
{
    public class RegisterUserModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string PersonalNumber { get; set; }
        [Required]
        public int? GenderId { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }

        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
