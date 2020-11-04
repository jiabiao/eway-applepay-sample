using Ardalis.Specification;
using eWAY.Samples.MonkeyStore.Models;
using System;
using System.Linq;

namespace eWAY.Samples.MonkeyStore.Web.Specifications
{
    public class ProductSpecification : Specification<Product>
    {
        public ProductSpecification(params int[] ids)
        {
            Query.Where(c => ids.Contains(c.Id));
        }
    }
}
