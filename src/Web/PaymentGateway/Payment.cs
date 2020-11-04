namespace eWAY.Samples.MonkeyStore.PaymentGateway
{
    public class Payment
    {
        public decimal TotalAmount { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDescription { get; set; }
        public string InvoiceReference { get; set; }
        public string CurrencyCode { get; set; }
    }
}
