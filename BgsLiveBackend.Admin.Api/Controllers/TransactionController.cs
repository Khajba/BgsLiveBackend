using Bgs.Live.Bll.Abstract;
using BgsLiveBackend.Admin.Api.Models.Transaction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BgsLiveBackend.Admin.Api.Controllers
{
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("getAll")]
        public IActionResult GetAll([FromQuery] TransactionFilterModel filter)
        {
            var transactions = _transactionService.GetTransactions(filter.TypeId,filter.DateFrom, filter.DateTo, filter.AmountFrom, filter.AmountTo, filter.PageNumber, filter.PageSize);

            return Ok(transactions);
        }

        [HttpGet("getTransactionsCount")]
        public IActionResult GetTransactionsCount([FromQuery] TransactionFilterModel filter)
        {
            var count = _transactionService.GetTransactionsCount(filter.TypeId, filter.DateFrom, filter.DateTo, filter.AmountFrom, filter.AmountTo);

            return Ok(count);
        }
    }
}
