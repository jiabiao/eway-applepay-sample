using Ardalis.Specification;
using eWAY.Samples.MonkeyStore.Models;
using System;
using System.Linq;

namespace eWAY.Samples.MonkeyStore.Web.Specifications
{
    public class TransactionSpecification : Specification<Transaction>
    {
        public TransactionSpecification(params int[] ids)
        {
            Query.Where(c => ids.Contains(c.Id));
        }
    }
}
