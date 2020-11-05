// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.PaymentGateway.Options
{
    public class GatewayCredential
    {
        public const string DEFAULT_SECTION = "Payment:Gateway:Credential";

        public string Key { get; set; }

        public string Secret { get; set; }
    }
}
