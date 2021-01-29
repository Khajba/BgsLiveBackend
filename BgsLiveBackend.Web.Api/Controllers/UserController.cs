using Bgs.Live.Bll.Abstract;
using BgsLiveBackend.Web.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BgsLiveBackend.Web.Api.Controllers
{
    public class UserController : BgsLiveController
    {
        private readonly IUserService _UserService;
        private readonly ITransactionService _TransactionService;

        public UserController(IUserService userService, ITransactionService transactionService)
        {
            _UserService = userService;
            _TransactionService = transactionService;
        }

        [HttpGet("getDetails")]
        public async Task<IActionResult> GetDetails()
        {
            throw new Exception("sdd");
            //var userDetails = await _UserService.GetUserDetails(CurrentUserId);

           // return Ok(userDetails);
        }

        [HttpPost("update")]
        public async Task<IActionResult> SaveDetails(UpdateUserDetailsModel model)
        {            

            await _UserService.SaveDetails(CurrentUserId, model.Firstname, model.Lastname, model.BirthDate, model.GenderId, model.Address, model.PhoneNumber);
            return Ok();
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordModel model)
        {
            await _UserService.ChangeUserPassword(CurrentUserId, model.OldPassword, model.NewPassword);
            return Ok();
        }

        [HttpPost("addBalance")]
        public async Task<IActionResult> AddBalance([FromBody][Required] decimal amount)
        {
            await _UserService.AddBalance(CurrentUserId, amount);
            return Ok();
        }

        [HttpPost("withdrow")]
        public async Task<IActionResult> Withdrow([FromBody][Required] decimal amount)
        {
            await _UserService.Withdrow(CurrentUserId, amount);
            return Ok();
        }

        [HttpGet("getBalance")]
        public async Task<IActionResult> GetBalance()
        {
            var balance = await _UserService.GetBalance(CurrentUserId);
            return Ok(balance);
        }

        [HttpPost("uploadUserAvatar")]
        public async Task<IActionResult> UploadUserAvatar(IFormFile file)
        {
            var avatarUrl = await _UserService.UploadUserAvatar(CurrentUserId, file);
            return Ok(new UploadUserAvatarResponseModel { AvatarUrl = avatarUrl });
        }

        [HttpPost("removeAvatar")]
        public async Task<IActionResult> RemoveAvatar()
        {
            await _UserService.DeleteAvatar(CurrentUserId);

            return Ok();
        }

        [HttpGet("getTransactions")]
        public async Task<IActionResult> GetTransactions([FromQuery]TransactionFilterModel filter)
        {
            var transactions = await _TransactionService.GetTransactions(
                filter.TypeId, 
                filter.DateFrom, 
                filter.DateTo, 
                filter.Amountfrom,
                filter.AmountTo, 
                filter.PageNumber,
                filter.PageSize);

            return Ok(transactions);
        }

        [HttpGet("getTransactionsCount")]
        public async Task<IActionResult> GetTransactionsCount([FromQuery] TransactionFilterModel filter)
        {
            var count = await _TransactionService.GetTransactionsCount(
                filter.TypeId, 
                filter.DateFrom, 
                filter.DateTo, 
                filter.Amountfrom,
                filter.AmountTo);

            return Ok(count);
        }
    }
}
