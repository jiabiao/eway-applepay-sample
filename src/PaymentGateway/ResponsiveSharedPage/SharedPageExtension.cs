// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.DependencyInjection.Extensions;
using MonkeyStore.PaymentGateway.Services;
using MonkeyStore.PaymentGateway.SharedPage;
using MonkeyStore.PaymentGateway.SharedPage.Messages;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SharedPage
    {
        public static IServiceCollection AddSharedPage(this IServiceCollection services)
        {
            services.TryAddScoped<SharedPageService>();
            services.TryAddScoped<IAccessCodeSharedProvider<CreateAccessCodeSharedResponse>, SharedPageService>();
            return services;
        }
    }
}
