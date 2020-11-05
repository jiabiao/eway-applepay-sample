// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.PaymentGateway.Options
{
    public class GatewayEndpoints
    {
        public const string DEFAULT_SECTION = "Payment:Gateway:Endpoints";

        public string DirectPayment { get; set; }

        public string TransparentRedirect { get; set; }

        public string ResponsiveSharedPage { get; set; }
    }
}
