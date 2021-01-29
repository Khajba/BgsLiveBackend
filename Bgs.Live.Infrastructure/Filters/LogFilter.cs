using Bgs.Live.Bll.Abstract;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bgs.Live.Infrastructure.Filters
{
    public class LogFilter : ActionFilterAttribute
    {
        private readonly ILogService _logService;
        public LogFilter(ILogService logService)
        {
            _logService = logService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Task.Run(async () =>
            {
                var url = context.HttpContext.Request.Path.Value;
                var ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
                var browser = context.HttpContext.Request.Headers["User-Agent"].ToString();
                var query = context.HttpContext.Request.QueryString.Value.Trim('?');
                string param;

                using (var reader = new StreamReader(context.HttpContext.Request.Body))
                {
                    param = await reader.ReadToEndAsync();
                }
                await _logService.AddLogRequest(url, DateTime.Now, ip, browser, query, param);

            });
        }

    }
}
