using System.Collections.Generic;

namespace eWAY.Samples.MonkeyStore.PaymentGateway.TranparentRedirect
{
    public class GenerateAccessCodeRequest: PaymentRequest
    {
        public Customer Customer { get; set; }
        public Address ShippingAddress { get; set; }
        public List<SkuItem> Items { get; set; }
        public Dictionary<string, string> Options { get; set; }
        public Payment Payment { get; set; }
        public string RedirectUrl { get; set; }
        public string DeviceID { get; set; }
        public string CustomerIP { get; set; }
        public string PartnerID { get; set; }
    }
}
