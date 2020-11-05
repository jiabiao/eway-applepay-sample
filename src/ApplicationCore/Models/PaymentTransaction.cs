// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using MonkeyStore.Models.Abstractions;
using MonkeyStore.PaymentGateway;

namespace MonkeyStore.Models
{
    public class PaymentTransaction : BaseEntity
    {
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public GatewayType GatewayType { get; set; }

        /// <summary>
        /// The access code used to query the payment result from eway platform.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The transaction ID on the payment gateway platform
        /// </summary>

        public string ExternalTransactionId { get; set; }
    }
}
