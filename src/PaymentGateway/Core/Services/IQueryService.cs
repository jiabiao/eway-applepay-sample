// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using System.Threading.Tasks;

namespace MonkeyStore.PaymentGateway.Services
{
    public interface IQueryService
    {
        Task<string> GetByAccessCodeAsync(string accessCode);

        Task<string> GetByTransactionId(string id);
    }
}
