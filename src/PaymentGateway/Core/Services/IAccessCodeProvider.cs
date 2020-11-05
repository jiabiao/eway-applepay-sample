// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using MonkeyStore.PaymentGateway.Messages;
using System.Threading.Tasks;

namespace MonkeyStore.PaymentGateway.Services
{
    public interface IAccessCodeProvider
    {
        Task<CreateAccessCodeResponse> CreateAccessCodeAsync(CreateAccessCodeRequest request);
    }
}
