using System;
using System.Threading.Tasks;

namespace Bgs.Dal.Abstract
{
    public interface ILogRepository
    {
        public Task AddLogError(DateTime logDate, string message, string stackTrace);
        public Task AddLogRequest(string url, DateTime logDate, string ip, string browser, string query, string param);


    }


}
