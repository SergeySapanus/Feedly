using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyFeedlyServer.Extensions;
using NLog.Extensions.Logging;

namespace MyFeedlyServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            loggerFactory.ConfigureNLog(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureSyndicationManager();
            services.ConfigureMsSqlContext(Configuration);
            services.ConfigureRepositoryWrapper();
            services.ConfigureFilterAttributes();
            services.ConfigureAuthentication();
            services.ConfigureSwagger();
            services.ConfigureDataProtector();
            services.ConfigureMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(ServiceExtensions.CORS_POLICY);

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseStaticFiles();
            app.UseCustomExceptionMiddleware();
            app.UseAuthentication();
            app.UseCustomSwagger();
            app.UseMvc();
        }
    }
}
