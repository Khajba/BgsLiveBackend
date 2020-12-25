using Bgs.Live.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Bll.Abstract
{
    public interface IRouletteService
    {
        public Task<BetResponseModel> Bet(int userId, decimal amount, decimal number);
    }
}
