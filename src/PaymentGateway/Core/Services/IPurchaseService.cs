// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using MonkeyStore.PaymentGateway.Messages;
using System.Threading.Tasks;

namespace MonkeyStore.PaymentGateway.Services
{
    public interface IPurchaseService<TRequest, TResponse>
               where TRequest : RequestBase
               where TResponse : ResponseBase
    {
        Task<TResponse> PurchaseAsync(TRequest request);
    }
}
