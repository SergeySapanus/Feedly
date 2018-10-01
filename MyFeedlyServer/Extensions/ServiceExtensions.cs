﻿using Contracts;
using Contracts.Repositories;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFeedlyServer.CustomExceptionMiddleware;
using Repository;
using SyndicationService;

namespace MyFeedlyServer.Extensions
{
    public static class ServiceExtensions
    {
        internal const string CORS_POLICY = "CorsPolicy";

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CORS_POLICY,
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureMsSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ConnectionString");
            services.AddDbContext<RepositoryContext>(o => o.UseLazyLoadingProxies(false).UseSqlServer(connectionString));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<ISyndicationManager, SyndicationManager>();
        }

        public static void ConfigureMvc(this IServiceCollection services)
        {
            services
                .AddMvc()
                .AddJsonOptions(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}