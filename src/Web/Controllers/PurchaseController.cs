using eWAY.Samples.MonkeyStore.Models;
using eWAY.Samples.MonkeyStore.Models.ApplePay;
using eWAY.Samples.MonkeyStore.PaymentGateway;
using eWAY.Samples.MonkeyStore.PaymentGateway.DirectPayment;
using eWAY.Samples.MonkeyStore.PaymentGateway.Services;
using eWAY.Samples.MonkeyStore.PaymentGateway.TranparentRedirect;
using eWAY.Samples.MonkeyStore.Repositories;
using eWAY.Samples.MonkeyStore.Web;
using eWAY.Samples.MonkeyStore.Web.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eWAY.Samples.MonkeyStore.Controllers
{
    [Route("api/{controller}/{action}")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger<PurchaseController> logger;
        private readonly IAsyncRepository<Transaction> transactionRepository;
        private readonly IAsyncRepository<Product> productRepository;
        private readonly IPaymentService paymentService;

        public PurchaseController(IAsyncRepository<Product> productRepository,
                                  IAsyncRepository<Transaction> transactionRepository,
                                  IPaymentService paymentService,
                                  IHttpClientFactory clientFactory,
                                  ILogger<PurchaseController> logger)
        {
            this.productRepository = productRepository;
            this.transactionRepository = transactionRepository;
            this.paymentService = paymentService;
            this.clientFactory = clientFactory;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Transaction>> GetTransactionAsync(int id)
        {
            var spec = new TransactionSpecification(new[] { id });
            var transaction = await transactionRepository.FirstOrDefaultAsync(spec);

            if (transaction == null)
                return NotFound();

            return transaction;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Transaction>> ListAsync()
        {
            return await transactionRepository.ListAllAsync();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Transaction>> PayAsync(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var spec = new ProductSpecification(new[] { order.ProductId });
            var product = await productRepository.FirstOrDefaultAsync(spec);
            if (product == null)
            {
                return BadRequest();
            }

            if (order.PaymentMethod != Models.PaymentMethod.ApplePay)
            {
                return BadRequest("Unsupported payment method.");
            }

            if (order.ApplePayPaymentToken == null)
            {
                return BadRequest();
            }

            var response = await paymentService.PurchaseAsync(new DirectPaymentRequest
            {
                ApplePay = order.ApplePayPaymentToken.PaymentData
            });

            var transaction = new Transaction
            {
                CreatedAt = DateTime.Now,
                Product = product,
                ApplePayPaymentToken = JsonSerializer.Serialize(order.ApplePayPaymentToken),
                PaymentResponse = JsonSerializer.Serialize(response),
            };

            await transactionRepository.AddAsync(transaction);

            return CreatedAtAction(nameof(GetTransactionAsync), new { transaction.Id }, transaction);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenerateAccessCodeResponse>> AccessCodeAsync(Order order)
        {
            var spec = new ProductSpecification(new[] { order.ProductId });
            var product = await productRepository.FirstOrDefaultAsync(spec);
            if (product == null)
            {
                return BadRequest();
            }

            var accessCodeRequest = new GenerateAccessCodeRequest
            {
                Customer = new Customer
                {
                    Title = "Mr.",
                    City = "EMU CREEK",
                    State = "Victoria",
                    Country = "AU",
                    FirstName = "Jane",
                    LastName = "Doe",
                    Phone = "00 00 00 00",
                    PostalCode = "00 00",
                    Street1 = "12 Redesdale Rd",
                },
                ShippingAddress = new Address
                {
                    City = "EMU CREEK",
                    State = "Victoria",
                    Country = "AU",
                    FirstName = "Jane",
                    LastName = "Doe",
                    Phone = "00 00 00 00",
                    PostalCode = "00 00",
                    Street1 = "12 Redesdale Rd",
                },
                CustomerIP = HttpContext.Connection.RemoteIpAddress.ToString(),
                RedirectUrl = "https://eway.io",
                Items = new List<SkuItem>
                {
                    new SkuItem
                    {
                        SKU = product.Text,
                        Description = product.Text,
                        Quantity = 1,
                        UnitCost = product.Price,
                        Tax = 0,
                        Total = product.Price * 1
                    }
                },
                Payment = new Payment
                {
                    CurrencyCode = "AUD",
                    TotalAmount = product.Price
                }
            };

            var response = await paymentService.CreateAccessTokenAsync(accessCodeRequest);

            return response;
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Validate([FromBody] MerchantValidationRequest sessionModel)
        {
            // You may wish to additionally validate that the URI specified for merchant validation in the
            // request body is a documented Apple Pay JS hostname. The IP addresses and DNS hostnames of
            // these servers are available here: https://developer.apple.com/documentation/applepayjs/setting_up_server_requirements
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Create the JSON payload to POST to the Apple Pay merchant validation URL.
            var request = new MerchantSessionRequest()
            {
                DisplayName = "Monkey Store",
                Initiative = "web",
                InitiativeContext = Request.GetTypedHeaders().Host.Value,
                MerchantIdentifier = ApplePayCertificates.MerchantIdentifier,
            };

            var client = clientFactory.CreateClient(Constants.NamedHttpClientApplePay);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var jsonContent = JsonSerializer.Serialize(request, jsonOptions);
            var requestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(sessionModel.ValidationUrl),
                Method = HttpMethod.Post,
                Content = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            try
            {
                logger.LogInformation("start to merchant validation.");
                var response = await client.SendAsync(requestMessage);
                logger.LogInformation("merchant validation responsed from Apple.");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var merchantSession = JsonSerializer.Deserialize<MerchantSessionResponose>(json, jsonOptions);
                return Ok(merchantSession);
            }
            catch (HttpRequestException webException)
            {
                logger.LogError(webException, "merchant validation error");
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "something goes wrong.");
            }
            logger.LogError("something goes wrong. return 504 status code to client");
            return StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }
    }
}
