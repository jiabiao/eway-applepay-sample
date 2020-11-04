using System;

namespace eWAY.Samples.MonkeyStore.Models
{
    public class Transaction: BaseEntity
    {
        public Product Product { get; set; }

        public string ApplePayPaymentToken { get; set; }

        public string PaymentResponse { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
