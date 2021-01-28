// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.PaymentGateway.Services
{
    public interface IUriComposer
    {
        string Build(string template, params (string name, string value)[] @params);
    }
}
