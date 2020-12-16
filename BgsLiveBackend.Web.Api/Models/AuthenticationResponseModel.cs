using Bgs.Infrastructure.Api.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BgsLiveBackend.Web.Api.Models
{
    public class AuthenticationResponseModel
    {
        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public JsonWebToken Jwt { get; set; }
    }
}
