
using Bgs.Live.Bll.Abstract;
using Bgs.Live.Core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace Bgs.Infrastructure.Api.Exceptions
{
    public class GlobalExceptionHandler
    {
        private readonly IWebHostEnvironment _environment;
        private readonly RequestDelegate _next;
        private readonly ILogService _logService;


        public GlobalExceptionHandler(RequestDelegate next, IWebHostEnvironment environment, ILogService logService)
        {
            _environment = environment;
            _next = next;
            _logService = logService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {     




            try
            {
                await _next(httpContext);
                
            }
            catch (BgsException ex)
            {
                await HandleExceptionAsync(httpContext, (int)HttpStatusCode.BadRequest, (int)ex.Errorcode);
            }
            catch (Exception ex)
            {
                Task.Run(async () =>
                {
                    await _logService.AddLogError(DateTime.Now, ex.Message, ex.StackTrace);
                });

                if (_environment.IsDevelopment())
                {
                    throw ex;
                }

                await HandleExceptionAsync(httpContext, (int)HttpStatusCode.InternalServerError);


            }
        }

        private Task HandleExceptionAsync(HttpContext context, int statusCode, int? errorCode = null)
        {
            context.Response.ContentType = "Application/Json";
            context.Response.StatusCode = statusCode;

            var responseString = JsonConvert.SerializeObject(new
            {
                errorCode
            });

            return context.Response.WriteAsync(responseString);
        }
    }
}