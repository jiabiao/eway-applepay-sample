// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using MonkeyStore.PaymentGateway.Messages;
using System.Threading.Tasks;

namespace MonkeyStore.PaymentGateway.Services
{
    public interface IAccessCodeSharedProvider<TResponse> where TResponse: CreateAccessCodeResponse
    {
        Task<TResponse> CreateAccessCodeSharedAsync(CreateAccessCodeRequest request);
    }
}
