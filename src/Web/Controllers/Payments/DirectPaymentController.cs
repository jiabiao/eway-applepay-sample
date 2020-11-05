// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.AspNetCore.Mvc;
using MonkeyStore.DTOs.Payments.DirectPayment;
using MonkeyStore.Models;
using MonkeyStore.PaymentGateway;
using MonkeyStore.PaymentGateway.DirectPayment.Messages;
using MonkeyStore.PaymentGateway.Services;
using MonkeyStore.Repositories;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonkeyStore.Controllers.Payments
{
    [Route(Constants.ROUTE_TEMPLATE_PAYMENT_GATEWAY)]
    [ApiController]
    public class DirectPaymentController : ControllerBase
    {
        private readonly IAsyncRepository<Product> _productRepository;
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IAsyncRepository<PaymentTransaction> _transactionRepository;
        private readonly IPurchaseService<DirectPaymentRequest, DirectPaymentResponse> _purchaseService;
        private readonly JsonSerializerOptions _serializerOptions;

        public DirectPaymentController(IAsyncRepository<Product> productRepository,
                                  IAsyncRepository<Order> orderRepository,
                                  IAsyncRepository<PaymentTransaction> transactionRepository,
                                  IPurchaseService<DirectPaymentRequest, DirectPaymentResponse> purchaseService)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _purchaseService = purchaseService;

            _serializerOptions = new JsonSerializerOptions
            {
                // PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
            _serializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseAsync([FromBody] PurchaseRequest purchaseRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = await _productRepository.GetByIdAsync(purchaseRequest.ProductId);

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

            var accessCodeRequest = new DirectPaymentRequest
            {
                Payment = new Payment
                {
                    CurrencyCode = "AUD",
                    TotalAmount = (int)(newOrder.Price * 100) * newOrder.Unit
                },
                PaymentInstrument = new PaymentInstrument
                {
                    PaymentType = PaymentType.ApplePay,
                    WalletDetails = new WalletDetails
                    {
                        Token = JsonSerializer.Serialize(purchaseRequest.ApplePayToken.PaymentData, _serializerOptions)
                    }
                }
            };

            var newTransaction = await _transactionRepository.AddAsync(new PaymentTransaction
            {
                OrderId = newOrder.Id,
                GatewayType = GatewayType.DirectPayment,
            });

            var purchaseResponse = await _purchaseService.PurchaseAsync(accessCodeRequest);

            if (!string.IsNullOrEmpty(purchaseResponse.Errors)
              || string.IsNullOrEmpty(purchaseResponse.TransactionId))
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }

            newTransaction.ExternalTransactionId = purchaseResponse.TransactionId;

            return Ok(new PurchaseResponse { OrderId = newOrder.Id });
        }
    }
}
