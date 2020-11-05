// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.DTOs.ApplePay
{
    public class MerchantSessionResponose
    {
        public long EpochTimestamp { get; set; }

        public long ExpiresAt { get; set; }

        public string MerchantSessionIdentifier { get; set; }

        public string Nonce { get; set; }

        public string MerchantIdentifier { get; set; }

        public string DomainName { get; set; }

        public string DisplayName { get; set; }

        public string Signature { get; set; }
    }
}
