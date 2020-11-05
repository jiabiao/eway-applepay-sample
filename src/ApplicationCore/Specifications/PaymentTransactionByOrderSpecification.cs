// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Ardalis.Specification;
using MonkeyStore.Models;
using System.Linq;

namespace MonkeyStore.Specifications
{
    public class PaymentTransactionByOrderSpecification : Specification<PaymentTransaction>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">Order id list</param>
        public PaymentTransactionByOrderSpecification(params int[] ids)
        {
            Query.Where(t => ids.Contains(t.OrderId));
        }
    }
}
