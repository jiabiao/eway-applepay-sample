// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.PaymentGateway.Options
{
    public class GatewayOptions
    {
        public const string DEFAULT_SECTION = "Payment:Gateway";

        public GatewayCredential Credential { get; set; }

        public string Endpoint { get; set; }
    }
}
