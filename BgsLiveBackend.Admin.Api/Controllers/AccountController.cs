using Bgs.Infrastructure.Api.Authorization;
using Bgs.Live.Bll.Abstract;
using BgsLiveBackend.Admin.Api.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BgsLiveBackend.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccountController : ControllerBase
    {

        private readonly IJwtHandler _jwtHandler;
        private readonly IInternalUserService _internalUserService;

       public AccountController(IJwtHandler jwtHandler, IInternalUserService internalUserService)
        {
            _jwtHandler = jwtHandler;
            _internalUserService = internalUserService;
        }

        [HttpGet("login")]
        public IActionResult Login([FromQuery] AuthenticateUserModel model)
        {
            var internalUser = _internalUserService.AuthenticateUser(model.Email, model.Password);

            var jwt = _jwtHandler.CreateToken(internalUser.Id);

            return Ok(new AuthenticationResponseModel
            {
                Email = internalUser.Email,
                Jwt = jwt
            });
        }

        [HttpGet("refreshToken")]
        [Authorize]
        public IActionResult RefreshToken()
        {
            var jwt = _jwtHandler.RefreshToken(User.Claims);

            return Ok(jwt);
        }
    }
}
