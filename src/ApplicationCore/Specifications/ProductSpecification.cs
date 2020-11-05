// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Ardalis.Specification;
using MonkeyStore.Models;
using System.Linq;

namespace MonkeyStore.Specifications
{
    public class ProductSpecification : Specification<Product>
    {
        public ProductSpecification(params int[] ids)
        {
            Query.Where(c => ids.Contains(c.Id));
        }
    }
}
