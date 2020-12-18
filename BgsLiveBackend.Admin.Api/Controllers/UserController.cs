using Bgs.Bll.Abstract;
using Bgs.Live.Bll.Abstract;
using BgsLiveBackend.Admin.Api.Models.User;
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

        [HttpGet("getusers")]
        public async Task<IActionResult> GetAll([FromQuery] UserFilterModel filter)
        {
            var users = await _userService.GetUsers(filter.PinCode, filter.Email, filter.Firstname, filter.Username, filter.Lastname, filter.PageNumber, filter.PageSize, filter.PersonalId);
            return Ok(users);
        }

        [HttpGet("getDetails")]
        public async Task<IActionResult> GetDetails([Required] int userId, [Required] int pageSize, [Required] int pageNumber)
        {
            var userDetails = await _userService.GetDetails(userId, pageSize, pageNumber);
            return Ok(userDetails);
        }

        [HttpGet("getUsersCount")]
        public async Task<IActionResult> GetUsersCount([FromQuery] UserFilterModel filter)
        {
            var count = await _userService.GetUsersCount(filter.PinCode, filter.Email, filter.Username, filter.Firstname, filter.Lastname, filter.PersonalId);
            return Ok(count);
        }
    }
}
