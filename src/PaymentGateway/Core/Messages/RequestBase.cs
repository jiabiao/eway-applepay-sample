// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.PaymentGateway.Messages
{
    public abstract class RequestBase
    {
        public string Method { get; protected set; }

        public string TransactionType { get; protected set; }

        protected RequestBase()
        {
            Method = "ProcessPayment";
        }

        protected RequestBase(string transactionType)
        {
            Method = "ProcessPayment";
            TransactionType = transactionType;
        }
    }
}
