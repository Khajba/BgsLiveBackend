using Bgs.Dal.Abstract;
using Bgs.Live.Bll.Abstract;
using Bgs.Live.Dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Bll
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task AddLogError(DateTime logDate, string message, string stackTrace)
        {
            await _logRepository.AddLogError(logDate, message, stackTrace);
        }

        public async Task AddLogRequest(string url, DateTime logDate, string ip, string browser, string query, string param)
        {
            await _logRepository.AddLogRequest(url, logDate, ip, browser, query, param);
        }
    }
}
