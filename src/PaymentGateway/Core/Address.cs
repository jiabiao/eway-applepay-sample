// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.PaymentGateway
{
    public class Address
    {
        public string ShippingMethod { get; protected set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }

        public Address()
        {
            ShippingMethod = "NextDay";
        }
    }
}
