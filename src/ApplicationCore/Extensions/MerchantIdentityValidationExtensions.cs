// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MonkeyStore;
using MonkeyStore.Options;
using MonkeyStore.Services;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MerchantIdentityValidationExtensions
    {

        /// <summary>
        /// Add required services for Apple Pay merchant identity validation.
        /// </summary>
        /// <remarks>
        /// This will Adds the follow items to the <see cref="IServiceCollection"/>:
        /// 
        /// <para>
        /// 1. <see cref="ApplePayOptions"/>
        /// </para>
        /// 
        /// <para>
        /// 2. <see cref="ApplePayCertificateProvider"/>
        /// </para>
        /// 
        /// <para>
        /// 3. <see cref="IHttpClientFactory"/> and related services and a named <see cref="HttpClient"/> 
        ///    with the name <see cref="Constants.NamedHttpClientApplePay"/>.
        /// </para>
        /// 
        /// <para>
        /// 4. <see cref="ApplePayMerchantValidator"/>
        /// </para>
        /// </remarks>
        public static void ConfigureMerchantIdentityValidation(this IServiceCollection services)
        {
            services.AddOptions<ApplePayOptions>()
                    .Configure<IConfiguration>((options, configuration) =>
                    {
                        configuration.GetSection(ApplePayOptions.DEFAULT_SECTION)
                                     .Bind(options);
                    })
                    .Validate(options => !string.IsNullOrEmpty(options.Thumbprint)
                                       , $"'{nameof(ApplePayOptions.Thumbprint)}' must be configured in order to do the merchant validation.")
                    .Validate(options => options.Certificates?.Count > 0
                                       , $"'{nameof(ApplePayOptions.Certificates)}' must be configured in order to do the merchant validation.");

            services.TryAddSingleton<ApplePayCertificateProvider>();

            // Typed HTTP client with the Two-Way TLS authentication
            // enabled for Apple Pay merchant identity validation.
            services.AddHttpClient(Constants.NamedHttpClientApplePay)
                    .ConfigurePrimaryHttpMessageHandler(sp =>
                    {
                        var certificateProvider = sp.GetRequiredService<ApplePayCertificateProvider>();
                        var certificate = certificateProvider.GetMerchantIdentityCertificate();
                        var handler = new HttpClientHandler();
                        handler.ClientCertificates.Add(certificate);
                        return handler;
                    });

            services.TryAddSingleton<ApplePayMerchantValidator>();
        }
    }
}
