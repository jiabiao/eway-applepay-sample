// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonkeyStore.Models;
using MonkeyStore.PaymentGateway;
using MonkeyStore.PaymentGateway.Messages;
using MonkeyStore.PaymentGateway.Services;
using MonkeyStore.PaymentGateway.SharedPage.Messages;
using MonkeyStore.Repositories;
using System.Net;
using System.Threading.Tasks;

namespace MonkeyStore.Pages.Payments.ResponsiveSharedPage
{
    public class CheckoutModel : PageModel
    {
        private readonly IAsyncRepository<Product> _productRepository;
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IAsyncRepository<PaymentTransaction> _transactionRepository;
        private readonly IAccessCodeSharedProvider<CreateAccessCodeSharedResponse> _accessCodeService;

        public CheckoutModel(IAsyncRepository<Product> productRepository,
                             IAsyncRepository<Order> orderRepository,
                             IAsyncRepository<PaymentTransaction> transactionRepository,
                             IAccessCodeSharedProvider<CreateAccessCodeSharedResponse> accessCodeProvider)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _accessCodeService = accessCodeProvider;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery(Name = "id")] int productId)
        {
            if (productId <= 0)
            {
                return BadRequest();
            }

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return BadRequest();
            }

            var newOrder = await _orderRepository.AddAsync(new Order
            {
                Price = product.Price,
                ProductId = product.Id,
                Unit = 1,
            });

            var callbackPath = Url.Page("/Payments/Result");
            var accessCodeRequest = new CreateAccessCodeRequest
            {
                RedirectUrl = Request.BuildCallbackLink(callbackPath, newOrder.Id),
                Payment = new Payment
                {
                    CurrencyCode = "AUD",
                    TotalAmount = (int)(newOrder.Price * 100) * newOrder.Unit
                }
            };
            var accessCodeSharedResponse = await _accessCodeService.CreateAccessCodeSharedAsync(accessCodeRequest);

            if (!string.IsNullOrEmpty(accessCodeSharedResponse.Errors))
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }

            await _transactionRepository.AddAsync(new PaymentTransaction
            {
                OrderId = newOrder.Id,
                GatewayType = GatewayType.ResponsiveSharedPage,
                Token = accessCodeSharedResponse.AccessCode
            });

            return Redirect(accessCodeSharedResponse.SharedPaymentUrl);
        }
    }
}
