// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MonkeyStore
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
            // Search all the assemblies in the current app domain to inject all the mapping profiles
            // between model and DTO classes which inherit from Profile class in AutoMapper
            var assemblies = AppDomain.CurrentDomain
                                      .GetAssemblies()
                                      .Where(assembly => assembly.FullName.StartsWith(nameof(MonkeyStore)))
                                      .ToArray();
            services.AddAutoMapper(assemblies);

            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                        options.JsonSerializerOptions.IgnoreNullValues = true;
                    });
            services.AddRazorPages()
                    .AddRazorRuntimeCompilation();

            ConfigureAppServices(services);
        }

        private void ConfigureAppServices(IServiceCollection services)
        {
            services.ConfigureCatalogDataStore();

            services.ConfigureMerchantIdentityValidation();

            services.ConfigurePaymentGateway()
                    .AddTransparentRedirect()
                    .AddDirectPayment()
                    .AddSharedPage();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
