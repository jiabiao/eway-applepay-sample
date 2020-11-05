// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using System.Collections.Generic;

namespace MonkeyStore.PaymentGateway.Messages
{
    public class CreateAccessCodeRequest : RequestBase
    {
        public Customer Customer { get; set; }
        public Address ShippingAddress { get; set; }
        public List<SkuItem> Items { get; set; }
        public Payment Payment { get; set; }
        public string RedirectUrl { get; set; }

        public CreateAccessCodeRequest()
        {
            TransactionType = "Purchase";
        }

        public CreateAccessCodeRequest(string transactionType) : base(transactionType)
        {}
    }
}
