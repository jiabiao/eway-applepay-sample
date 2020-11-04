namespace eWAY.Samples.MonkeyStore.PaymentGateway
{
    public abstract class PaymentResponse
    {
        public string AccessCode { get; set; }

        public string Status { get; set; }

        public string Errors { get; set; }
    }
}
