// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Unit { get; set; }
    }
}
