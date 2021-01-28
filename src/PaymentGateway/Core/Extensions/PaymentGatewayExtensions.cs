// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MonkeyStore.PaymentGateway;
using MonkeyStore.PaymentGateway.Options;
using MonkeyStore.PaymentGateway.Services;
using System;

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
            services.TryAddScoped<IUriComposer, DefaultUriComposer>();
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

            services.AddOptions<GatewayOptions>()
                    .Configure<IConfiguration>((options, configuration) =>
                    {
                        configuration.GetSection(GatewayOptions.DEFAULT_SECTION)
                                     .Bind(options);
                    })
                    .Validate(options => options.Credential != null
                                       , $"'{nameof(GatewayOptions.Credential)}' must be configured for the gateway.")
                    .Validate(options => options.Endpoint != null
                                       , $"'{nameof(GatewayOptions.Endpoint)}' must be configured for the gateway.")
                    .Validate(options => Uri.TryCreate(options.Endpoint, UriKind.Absolute, out Uri uri)
                                         && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                                       , $"'{nameof(GatewayOptions.Endpoint)}' must be a valid url endpoint for the gateway.");
        }
    }
}
