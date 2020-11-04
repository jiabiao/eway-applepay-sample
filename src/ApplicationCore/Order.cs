using eWAY.Samples.MonkeyStore.Models.ApplePay;
using System.ComponentModel.DataAnnotations;

namespace eWAY.Samples.MonkeyStore.Models
{
    public class Order
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public PaymentToken ApplePayPaymentToken { get; set; }
    }
}
