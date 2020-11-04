namespace eWAY.Samples.MonkeyStore.PaymentGateway
{
    public class SkuItem
    {
        public string SKU { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public int Tax { get; set; }
        public decimal Total { get; set; }
    }
}
