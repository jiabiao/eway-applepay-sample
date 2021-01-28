// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using MonkeyStore.PaymentGateway.Messages;

namespace MonkeyStore.PaymentGateway.SharedPage.Messages
{
    public class CreateAccessCodeSharedResponse : CreateAccessCodeResponse
    {
        public string SharedPaymentUrl { set; get; }
    }
}
