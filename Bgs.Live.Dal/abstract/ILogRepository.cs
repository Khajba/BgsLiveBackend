using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Dal.Abstract
{
    public interface ILogRepository
    {
        public Task AddLogError(DateTime logDate, string message, string stackTrace);
    }
}
