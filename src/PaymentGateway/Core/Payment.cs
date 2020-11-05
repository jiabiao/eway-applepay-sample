// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.PaymentGateway
{
    public class Payment
    {
        /// <summary>
        /// The amount of the transaction in the lowest denomination for the currency.
        /// Reference Currency list - ISO 4127 Standard
        /// </summary>
        /// <remarks>
        /// Example:
        /// [1] For AUD, NZD, USD etc.These currencies have a decimal part: a $27.00 AUD transaction would have a TotalAmount = '2700'
        /// [2] For VND, JPY, KRW etc.These currencies DO NOT have a decimal part: a 27 VND transaction would have TotalAmount = '27'
        /// </remarks>
        public int TotalAmount { get; set; }

        /// <summary>
        /// The ISO 4217 3 character code that represents the currency that this transaction is to be processed in. 
        /// If no value for this field is provided, the merchant's default currency is used. 
        /// This should be in uppercase. e.g.Australian Dollars = AUD
        /// </summary>
        public string CurrencyCode { get; set; }
    }
}
