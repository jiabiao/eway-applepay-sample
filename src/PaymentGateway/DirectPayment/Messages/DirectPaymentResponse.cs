// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using MonkeyStore.PaymentGateway.Messages;

namespace MonkeyStore.PaymentGateway.DirectPayment.Messages
{
    public class DirectPaymentResponse : ResponseBase
    {
        public string TransactionStatus { get; set; }

        public string AdditionalAction { get; set; }

        public string TransactionId { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public string AuthorisationCode { get; set; }

        public string RelatedTransactionId { get; set; }

        public string Metadata { get; set; }
    }
}
