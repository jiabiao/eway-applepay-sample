// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.PaymentGateway.Messages
{
    public class CreateAccessCodeResponse : ResponseBase
    {
        public string AccessCode { get; set; }
    }
}
