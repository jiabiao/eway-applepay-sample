// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MonkeyStore.PaymentGateway;
using MonkeyStore.PaymentGateway.Options;
using MonkeyStore.PaymentGateway.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PaymentGatewayExtensions
    {
        /// <summary>
        /// Add the <see cref="GatewayAuthenticationBuilder"/> and options by call the method <see cref="ConfigurePaymentGatewayOptions"/>,
        /// including <see cref="GatewayCredential"/>, <see cref="GatewayEndpoints"/> and <see cref="GatewayOptions"/>
        /// </summary>
        public static IServiceCollection ConfigurePaymentGateway(this IServiceCollection services)
        {
            services.ConfigurePaymentGatewayOptions();
            services.TryAddSingleton<GatewayAuthenticationBuilder>();
            services.TryAddScoped<IQueryService, DefaultQueryService>();
            return services;
        }

        /// <summary>
        /// Add the gateway options: <see cref="GatewayCredential"/>, <see cref="GatewayEndpoints"/> and <see cref="GatewayOptions"/>.
        /// </summary>
        public static void ConfigurePaymentGatewayOptions(this IServiceCollection services)
        {
            services.AddOptions<GatewayCredential>()
                    .Configure<IConfiguration>((options, configuration) =>
                    {
                        configuration.GetSection(GatewayCredential.DEFAULT_SECTION)
                                     .Bind(options);
                    })
                    .Validate(options => !string.IsNullOrEmpty(options.Key)
                                       , $"'{nameof(GatewayCredential.Key)}' must be configured for the gateway credential.")
                    .Validate(options => !string.IsNullOrEmpty(options.Secret)
                                       , $"'{nameof(GatewayCredential.Secret)}' must be configured for the gateway credential.");

            services.AddOptions<GatewayEndpoints>()
                    .Configure<IConfiguration>((options, configuration) =>
                    {
                        configuration.GetSection(GatewayEndpoints.DEFAULT_SECTION)
                                     .Bind(options);
                    })
                    .Validate(options => !string.IsNullOrEmpty(options.DirectPayment)
                                       , $"'{nameof(GatewayEndpoints.DirectPayment)}' must be configured for the gateway endpoints.")
                    .Validate(options => !string.IsNullOrEmpty(options.TransparentRedirect)
                                       , $"'{nameof(GatewayEndpoints.TransparentRedirect)}' must be configured for the gateway endpoints.")
                    .Validate(options => !string.IsNullOrEmpty(options.ResponsiveSharedPage)
                                       , $"'{nameof(GatewayEndpoints.ResponsiveSharedPage)}' must be configured for the gateway endpoints.");

            services.AddOptions<GatewayOptions>()
                    .Configure<IConfiguration>((options, configuration) =>
                    {
                        configuration.GetSection(GatewayOptions.DEFAULT_SECTION)
                                     .Bind(options);
                    })
                    .Validate(options => options.Credential != null
                                       , $"'{nameof(GatewayOptions.Credential)}' must be configured for the gateway.")
                    .Validate(options => options.Endpoints != null
                                       , $"'{nameof(GatewayOptions.Endpoints)}' must be configured for the gateway.");
        }
    }
}
