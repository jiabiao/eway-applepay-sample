namespace eWAY.Samples.MonkeyStore.Models.ApplePay
{
    public class MerchantSessionRequest
    {
        public string MerchantIdentifier { get; set; }

        public string DisplayName { get; set; }

        public string Initiative { get; set; }

        public string InitiativeContext { get; set; }
    }
}
