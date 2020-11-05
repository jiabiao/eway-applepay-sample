// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.PaymentGateway
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
