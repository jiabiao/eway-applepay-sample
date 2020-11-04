namespace eWAY.Samples.MonkeyStore.PaymentGateway
{
    public abstract class PaymentRequest
    {
        public string Method { get; protected set; }

        public string TransactionType { get; protected set; }

        protected PaymentRequest()
        {
            Method = "ProcessPayment";
            TransactionType = "Purchase";
        }
    }
}
