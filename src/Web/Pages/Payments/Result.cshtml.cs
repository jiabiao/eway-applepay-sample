// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonkeyStore.Models;
using MonkeyStore.PaymentGateway.Services;
using MonkeyStore.Repositories;
using MonkeyStore.Specifications;
using System.Threading.Tasks;

namespace MonkeyStore.Pages.Payments
{
    public class ResultModel : PageModel
    {
        private readonly IAsyncRepository<PaymentTransaction> _transactionRepository;
        private readonly IQueryService _queryService;

        public string PaymentStatus { get; set; }

        public ResultModel(IAsyncRepository<PaymentTransaction> transactionRepository,
                           IQueryService queryService)
        {
            _transactionRepository = transactionRepository;
            _queryService = queryService;
        }

        public async Task OnGetAsync([FromQuery(Name = "order")] int orderId)
        {
            var spec = new PaymentTransactionByOrderSpecification(new[] { orderId });
            var transaction = await _transactionRepository.FirstOrDefaultAsync(spec);
            if (transaction == null)
            {
                NotFound();
            }
            else
            {
                //TODO: You should design and implement a solution for payments.
                //      We just display the payment status in here by query the gateway.
                if (!string.IsNullOrEmpty(transaction.ExternalTransactionId))
                {
                    PaymentStatus = await _queryService.GetByTransactionId(transaction.ExternalTransactionId);
                }
                else
                {
                    PaymentStatus = await _queryService.GetByAccessCodeAsync(transaction.Token);
                }
            }
        }
    }
}
