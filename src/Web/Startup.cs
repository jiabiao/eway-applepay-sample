using eWAY.Samples.MonkeyStore.Data;
using eWAY.Samples.MonkeyStore.PaymentGateway;
using eWAY.Samples.MonkeyStore.PaymentGateway.Services;
using eWAY.Samples.MonkeyStore.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace eWAY.Samples.MonkeyStore.Web
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
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                        options.JsonSerializerOptions.IgnoreNullValues = true;
                    });
            services.AddRazorPages();

            services.AddHttpClient();

            services.AddDbContext<StoreContext>();

            services.Configure<PaymentGatewayOptions>(Configuration.GetSection(PaymentGatewayOptions.DEFAULT_SECTION));
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            ConfigureApplePayHttpClient(services);
        }

        /// <summary>
        /// Typed HTTP client with the Two-Way TLS authentication enabled for Apple Pay merchant validation.
        /// </summary>
        private void ConfigureApplePayHttpClient(IServiceCollection services)
        {
            ApplePayCertificates.LoadMerchantIdentifierCertificate(Configuration);
            var cert = ApplePayCertificates.MerchantIdentifierCertificate;
            services.AddHttpClient(Constants.NamedHttpClientApplePay, c => { })
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        var handler = new HttpClientHandler();
                        handler.ClientCertificates.Add(cert);
                        return handler;
                    });
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
