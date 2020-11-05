// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.DependencyInjection.Extensions;
using MonkeyStore.PaymentGateway.DirectPayment;
using MonkeyStore.PaymentGateway.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TransparentRedirectExtension
    {
        /// <summary>
        /// Add transparent redirect as a payment method
        /// </summary>
        public static IServiceCollection AddTransparentRedirect(this IServiceCollection services)
        {
            services.TryAddScoped<TransparentRedirectService>();
            services.TryAddScoped<IAccessCodeProvider, TransparentRedirectService>();
            return services;
        }
    }
}
