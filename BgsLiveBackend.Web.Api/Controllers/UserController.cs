using Bgs.Live.Bll.Abstract;
using BgsLiveBackend.Web.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BgsLiveBackend.Web.Api.Controllers
{
    public class UserController : BgsLiveController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getDetails")]
        public async Task<IActionResult> GetDetails()
        {
            var userDetails = await _userService.GetUserAccountDetails(CurrentUserId);

            return Ok(userDetails);
        }

        

        [HttpPost("updateDetails")]
        public async Task<IActionResult> SaveDetails(string phoneNumber)
        {
            await _userService.SaveDetails(CurrentUserId, phoneNumber);
            return Ok();
        }

        [HttpGet("getUserAddress")]
        public async Task<IActionResult> GetUserAddress()
        {
            var userAddress = await _userService.GetUserAddress(CurrentUserId);

            return Ok(userAddress);
        }

        [HttpPost("saveUserAddress")]
        public async Task<IActionResult> SaveUserAddress(string address)
        {
            await _userService.SaveUserAddress(CurrentUserId,address);
            return Ok();
        }     


        [HttpPost("changeUserPassword")]
        public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordModel model)
        {
            await _userService.ChangeUserPassword(CurrentUserId, model.OldPassword, model.NewPassword);
            return Ok();
        }

        [HttpPost("addBalance")]
        public async Task<IActionResult> AddBalance([FromBody][Required] decimal amount)
        {
            await _userService.AddBalance(CurrentUserId, amount);
            return Ok();
        }

        [HttpGet("getBalance")]
        public async Task<IActionResult> GetBalance()
        {
            var balance = await _userService.GetBalance(CurrentUserId);
            return Ok(balance);
        }

        [HttpPost("uploadUserAvatar")]
        public async Task<IActionResult> UploadUserAvatar(IFormFile file)
        {
            var avatarUrl = await _userService.UploadUserAvatar(CurrentUserId, file);
            return Ok(new UploadUserAvatarResponseModel { AvatarUrl = avatarUrl });
        }

        [HttpPost("removeAvatar")]
        public async Task<IActionResult> RemoveAvatar()
        {
            await _userService.DeleteAvatar(CurrentUserId);

            return Ok();
        }
    }
}
