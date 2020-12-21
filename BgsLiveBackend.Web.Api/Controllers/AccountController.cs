using Bgs.Infrastructure.Api.Authorization;
using Bgs.Live.Bll.Abstract;
using BgsLiveBackend.Web.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BgsLiveBackend.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtHandler _jwtHandler;

        public AccountController(IUserService userService, IJwtHandler jwtHandler)
        {
            _userService = userService;
            _jwtHandler = jwtHandler;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserModel model)
        {
           await _userService.RegisterUser(model.Email, model.Firstname, model.Username, model.Lastname, model.Password, model.PersonalNumber, model.GenderId.Value, model.BirthDate.Value, model.Address);
            return Ok();
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] LoginUserModel model)
        {
            var user = await _userService.AuthenticateUser(model.Username, model.Password);

            var jwt = _jwtHandler.CreateToken(user.Id);

            return Ok(new AuthenticationResponseModel
            {
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                PinCode = user.PinCode,
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
