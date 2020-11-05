// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonkeyStore.Data.Catalog;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.Threading.Tasks;

namespace MonkeyStore
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(typeof(Program));
                try
                {
                    logger.LogInformation("Migrate databases...");

                    var storeContext = serviceProvider.GetRequiredService<CatalogContext>();
                    storeContext.Database.Migrate();

                    logger.LogInformation("Seeding data into databases...");

                    await CatalogContextSeed.SeedAsync(storeContext, loggerFactory);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((builderContext, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();

                    var nlogConfigs = builderContext.Configuration.GetSection("NLog");
                    NLog.LogManager.Configuration = new NLogLoggingConfiguration(nlogConfigs);
                })
                .UseNLog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
