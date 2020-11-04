namespace eWAY.Samples.MonkeyStore.PaymentGateway
{
    public class PaymentGatewayOptions
    {
        public const string DEFAULT_SECTION = "PaymentGateway";
        public const string PATH_DP_PAYMENT = "Payment";
        public const string PATH_TR_ACCESS_CODE = "AccessCodes";

        public string Endpoint { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
    }
}
