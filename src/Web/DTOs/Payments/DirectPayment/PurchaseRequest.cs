// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using MonkeyStore.ApplePay;
using System.ComponentModel.DataAnnotations;

namespace MonkeyStore.DTOs.Payments.DirectPayment
{
    public class PurchaseRequest
    {
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// ApplePay is the only value
        /// </summary>
        public string PaymentMethod { get; set; }

        [Required]
        public PaymentToken ApplePayToken { get; set; }
    }
}
