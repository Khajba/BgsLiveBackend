using Bgs.Dal.Abstract;
using Bgs.DataConnectionManager.SqlServer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Dal
{
    public class LogRepository: SqlServerRepository, ILogRepository
    {
        private const string _schemalog = "log";
        public LogRepository(IConfiguration configuration)
             : base(configuration, configuration.GetConnectionString("MainDatabase"))
        {

        }

        public async Task AddLogError(DateTime logDate, string message, string stackTrace)
        {
            using (var cmd = GetSpCommand($"{_schemalog}.AddErrorLog"))
            {
                cmd.AddParameter("LogDate", logDate);
                cmd.AddParameter("Message", message);
                cmd.AddParameter("StackTrace", stackTrace);


                await cmd.ExecuteNonQueryAsync();
            };
        }
    }
}
