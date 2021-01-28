// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonkeyStore.DTOs.Payments.TransparentRedirect;
using MonkeyStore.Models;
using MonkeyStore.PaymentGateway;
using MonkeyStore.PaymentGateway.Constants;
using MonkeyStore.PaymentGateway.Messages;
using MonkeyStore.PaymentGateway.Services;
using MonkeyStore.Repositories;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MonkeyStore.Controllers.Payments
{
    [Route(Constants.ROUTE_TEMPLATE_PAYMENT_GATEWAY)]
    [ApiController]
    public class TransparentRedirectController : ControllerBase
    {
        private readonly IAsyncRepository<Product> _productRepository;
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IAsyncRepository<PaymentTransaction> _transactionRepository;
        private readonly IAccessCodeProvider _accessCodeService;
        private readonly IUriComposer _uriComposer;

        public TransparentRedirectController(IAsyncRepository<Product> productRepository,
                                  IAsyncRepository<Order> orderRepository,
                                  IAsyncRepository<PaymentTransaction> transactionRepository,
                                  IAccessCodeProvider accessCodeProvider,
                                  IUriComposer uriComposer)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _accessCodeService = accessCodeProvider;
            _uriComposer = uriComposer;
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InitialAsync([FromBody] InitialRequest initialRequest)
        {
            // You may wish to additionally validate that the URI specified for merchant validation in the
            // request body is a documented Apple Pay JS hostname. The IP addresses and DNS hostnames of
            // these servers are available here: https://developer.apple.com/documentation/applepayjs/setting_up_server_requirements
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = await _productRepository.GetByIdAsync(initialRequest.ProductId);

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
            var createAccessCodeResponse = await _accessCodeService.CreateAccessCodeAsync(accessCodeRequest);

            if (!string.IsNullOrEmpty(createAccessCodeResponse.Errors))
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }

            var newTransaction = await _transactionRepository.AddAsync(new PaymentTransaction
            {
                OrderId = newOrder.Id,
                GatewayType = GatewayType.TransparentRedirect,
                Token = createAccessCodeResponse.AccessCode
            });

            var responsePayload = new InitialResponse
            {
                Endpoint = _uriComposer.Build(ResourceTemplates.TEMPLATE_TRANSACTION_TR),
                AccessCode = createAccessCodeResponse.AccessCode,
                TransactionId = newTransaction.Id,
            };

            return Ok(responsePayload);
        }
    }
}
