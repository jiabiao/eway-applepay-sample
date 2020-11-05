// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.DTOs
{
    public class PaymentTransactionDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string Token { get; set; }
    }
}
