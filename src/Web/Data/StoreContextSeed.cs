using eWAY.Samples.MonkeyStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eWAY.Samples.MonkeyStore.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext storeContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                // TODO: Only run this if using a real database
                // context.Database.Migrate();
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
                    var log = loggerFactory.CreateLogger<StoreContextSeed>();
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
                new Product{ Text="Noise Canceling Headset", Image ="images/products/01.jpg", Price = 199.9M},
                new Product{ Text="All-in-one Computer", Image ="images/products/02.jpg", Price = 2989.49M},
                new Product{ Text="Smartphone", Image ="images/products/03.jpg", Price = 688.00M},
                new Product{ Text="Net Camera", Image ="images/products/04.jpg", Price = 59M},
                new Product{ Text="All-in-one Printer", Image ="images/products/05.jpg", Price = 399.89M},
                new Product{ Text="Smart TV Box", Image ="images/products/06.jpg", Price = 99M},
            };
        }
    }
}
