// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MonkeyStore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonkeyStore.Data.Catalog
{
    public class CatalogContextSeed
    {
        protected CatalogContextSeed()
        { }

        public static async Task SeedAsync(CatalogContext storeContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                if (!await storeContext.Products.AnyAsync())
                {
                    await storeContext.Products.AddRangeAsync(GetPreconfiguredProducts());

                    await storeContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<CatalogContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(storeContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product{ Name="Smart TV Box|On Sale", Image ="/images/products/06.jpg", Price = 0.01M},
                new Product{ Name="Noise Canceling Headset", Image ="/images/products/01.jpg", Price = 199.9M},
                new Product{ Name="All-in-one Computer", Image ="/images/products/02.jpg", Price = 2989.49M},
                new Product{ Name="Smartphone", Image ="/images/products/03.jpg", Price = 688.00M},
                new Product{ Name="Net Camera", Image ="/images/products/04.jpg", Price = 59M},
                new Product{ Name="All-in-one Printer", Image ="/images/products/05.jpg", Price = 399.89M},
                new Product{ Name="Smart TV Box", Image ="/images/products/06.jpg", Price = 99M},
            };
        }
    }
}
