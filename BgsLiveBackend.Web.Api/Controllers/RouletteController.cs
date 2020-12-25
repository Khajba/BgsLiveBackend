using Bgs.Live.Bll.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BgsLiveBackend.Web.Api.Controllers
{
    public class RouletteController : BgsLiveController
    {
        private readonly IRouletteService _RouletteService;

        public RouletteController(IRouletteService rouletteService)
        {
            _RouletteService = rouletteService;
        }

        [HttpGet("bet")]
        public async Task<IActionResult> Bet(decimal amount, decimal number)
        {
            var response = await _RouletteService.Bet(CurrentUserId, amount, number);
            return Ok(response);
        }
    }
}
