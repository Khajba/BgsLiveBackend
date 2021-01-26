using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Bll.Abstract
{
    public interface ILogService
    {
        public Task AddLogError(DateTime logDate, string message, string stackTrace);
    }
}
