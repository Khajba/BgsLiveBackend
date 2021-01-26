using Bgs.Dal.Abstract;
using Bgs.Infrastructure.Api.Authorization;
using Bgs.Infrastructure.Api.Exceptions;
using Bgs.Live.Bll;
using Bgs.Live.Bll.Abstract;
using Bgs.Live.Dal;
using Bgs.Live.Infrastructure.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BgsLiveBackend.Admin.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            // services
            
            services.AddSingleton<IInternalUserService, InternalUserService>();
            services.AddSingleton<ILogService, LogService>();

            // repositories
            services.AddSingleton<IInternalUserRepository, InternalUserRepository>();
            services.AddSingleton<ILogRepository, LogRepository>();



            services.AddHttpClient();

            services.AddControllers();
            services.AddBgsAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<RequestHandler>();
            app.UseMiddleware<GlobalExceptionHandler>();

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
