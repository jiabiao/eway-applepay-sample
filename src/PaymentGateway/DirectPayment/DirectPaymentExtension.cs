// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.DependencyInjection.Extensions;
using MonkeyStore.PaymentGateway.DirectPayment;
using MonkeyStore.PaymentGateway.DirectPayment.Messages;
using MonkeyStore.PaymentGateway.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DirectPaymentExtension
    {
        public static IServiceCollection AddDirectPayment(this IServiceCollection services)
        {
            services.TryAddScoped<DirectPaymentService>();
            services.TryAddScoped<IPurchaseService<DirectPaymentRequest, DirectPaymentResponse>, DirectPaymentService>();
            return services;
        }
    }
}
