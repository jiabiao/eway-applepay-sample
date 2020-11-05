// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MonkeyStore.Data.Catalog;
using MonkeyStore.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CatalogExtensions
    {
        /// <summary>
        /// Add the <see cref="IAsyncRepository&lt;TEntity&gt;"/> and <see cref="CatalogContext"/> for the catalog data store.
        /// </summary>
        public static void ConfigureCatalogDataStore(this IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>((serviceProvider, optionsBuilder) =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(nameof(CatalogExtensions));

                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connection = configuration.GetConnectionString(nameof(CatalogContext));
                logger.LogInformation($"Using connection '{connection}' for the SQLite.");
                optionsBuilder.UseSqlite(connection);
            });
            services.TryAddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
        }
    }
}
