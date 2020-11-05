// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using System.Collections.Generic;

namespace MonkeyStore.Options
{
    public class ApplePayOptions
    {
        public const string DEFAULT_SECTION = "Payment:ApplePay";

        /// <summary>
        /// Thumbprint for the Apple Pay merchant identity certificate current in use.
        /// </summary>
        public string Thumbprint { get; set; }

        /// <summary>
        /// Echo the Apple Pay payment token instead of redirect to result page.
        /// </summary>
        /// <remarks>
        /// <para>Only works for transparent redirect and direct payment demo solution.</para>
        /// </remarks>
        public bool EchoMode { get; set; }

        public List<MerchantIdentityCertificate> Certificates { get; set; }

        public class MerchantIdentityCertificate
        {
            public string FileName { get; set; }

            public string Password { get; set; }
        }
    }
}
