using Bgs.Bll.Abstract;
using Bgs.Dal.Abstract;
using Bgs.Infrastructure.Api.Authorization;
using Bgs.Infrastructure.Api.Exceptions;
using Bgs.Live.Bll;
using Bgs.Live.Bll.Abstract;
using Bgs.Live.Dal;
using Bgs.Live.Dal.Abstract;
using Bgs.Live.Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BgsLiveBackend.Web.Api
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

            //Services
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ITransactionService, TransactionService>();
            services.AddSingleton<ILogService, LogService>();

            // repositories            
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ITransactionRepository, TransactionRepository>();
            services.AddSingleton<ILogRepository, LogRepository>();


            services.AddHttpClient();

            services.AddControllers(options =>
            {
                options.Filters.Add<CommandActionFIlter>();
                options.Filters.Add<LogFilter>();
            });

            services.AddBgsAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMiddleware<RequestHandler>();
            app.UseMiddleware<GlobalExceptionHandler>();

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
