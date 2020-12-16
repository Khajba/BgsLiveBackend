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

        [HttpGet("getAccount")]
        public IActionResult GetDetails()
        {
            var userAccount = _userService.GetUserAccountDetails(CurrentUserId);

            return Ok(userAccount);
        }

        [HttpPost("saveDetails")]
        public IActionResult SaveDetails(SaveDetailsModel model)
        {
            _userService.SaveDetails(CurrentUserId, model.Firstname, model.Lastname);
            return Ok();
        }

        [HttpGet("getUserAddress")]
        public IActionResult GetUserAddress()
        {
            var userAddress = _userService.GetUserAddress(CurrentUserId);

            return Ok(userAddress);
        }

        [HttpPost("saveUserAddress")]
        public IActionResult SaveUserAddress(string address)
        {
            _userService.SaveUserAddress(CurrentUserId,address);
            return Ok();
        }     


        [HttpPost("changeUserPassword")]
        public IActionResult ChangeUserPassword(ChangeUserPasswordModel model)
        {
            _userService.ChangeUserPassword(CurrentUserId, model.OldPassword, model.NewPassword);
            return Ok();
        }

        [HttpPost("addBalance")]
        public IActionResult AddBalance([FromBody][Required] decimal amount)
        {
            _userService.AddBalance(CurrentUserId, amount);
            return Ok();
        }

        [HttpGet("getBalance")]
        public IActionResult GetBalance()
        {
            var balance = _userService.GetBalance(CurrentUserId);
            return Ok(balance);
        }

        [HttpPost("uploadUserAvatar")]
        public IActionResult UploadUserAvatar(IFormFile file)
        {
            var avatarUrl = _userService.UploadUserAvatar(CurrentUserId, file);
            return Ok(new UploadUserAvatarResponseModel { AvatarUrl = avatarUrl });
        }

        [HttpPost("removeAvatar")]
        public IActionResult RemoveAvatar()
        {
            _userService.DeleteAvatar(CurrentUserId);

            return Ok();
        }
    }
}
