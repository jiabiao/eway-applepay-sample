using eWAY.Samples.MonkeyStore.Models.ApplePay;

namespace eWAY.Samples.MonkeyStore.PaymentGateway.DirectPayment
{
    public class DirectPaymentRequest : PaymentRequest
    {
        public PaymentData ApplePay { get; set; }
    }
}
