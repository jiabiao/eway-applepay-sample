// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.DTOs.Payments.TransparentRedirect
{
    public class InitialRequest
    {
        public int ProductId { get; set; }

        /// <summary>
        /// Only ApplePay for now
        /// </summary>
        public string PaymentMethod { get; set; }
    }
}
