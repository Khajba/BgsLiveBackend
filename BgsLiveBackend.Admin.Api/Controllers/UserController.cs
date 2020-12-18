using Bgs.Bll.Abstract;
using Bgs.Live.Bll.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BgsLiveBackend.Admin.Api.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
            
        }

        [HttpGet("getDetails")]
        public async Task<IActionResult> GetDetails([Required] int userId, [Required] int pageSize, [Required] int pageNumber)
        {
            var userDetails = await _userService.GetDetails(userId, pageSize, pageNumber);
            return Ok(userDetails);
        }
    }
}
