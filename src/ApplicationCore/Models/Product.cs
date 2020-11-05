// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using MonkeyStore.Models.Abstractions;

namespace MonkeyStore.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }
    }
}
