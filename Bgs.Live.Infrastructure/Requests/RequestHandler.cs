using Bgs.Live.Bll.Abstract;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bgs.Live.Infrastructure.Requests
{
    public class RequestHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogService _logService;

        public RequestHandler(RequestDelegate next, ILogService logService)
        {
            _next = next;
            _logService = logService;
        }

        public async Task Invoke(HttpContext httpContext)
        {            

            Task.Run(async () =>
            {
                var url = httpContext.Request.Path.Value;
                var ip = httpContext.Connection.RemoteIpAddress.ToString();
                var browser = httpContext.Request.Headers["User-Agent"].ToString();
                var query = httpContext.Request.QueryString.Value.Trim('?');
                string param;

                using (var reader = new StreamReader(httpContext.Request.Body))
                {
                    param = await reader.ReadToEndAsync();
                }
                await _logService.AddLogRequest(url, DateTime.Now, ip, browser, query, param);
            });

            await _next(httpContext);
        }
    }
}
