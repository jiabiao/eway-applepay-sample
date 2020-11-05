// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.DTOs.Payments.TransparentRedirect
{
    public class InitialResponse
    {
        public string Endpoint { get; set; }

        public string AccessCode { get; set; }

        public int TransactionId { get; set; }
    }
}
