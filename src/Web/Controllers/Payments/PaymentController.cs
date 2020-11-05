// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonkeyStore.DTOs.ApplePay;
using MonkeyStore.Services;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MonkeyStore.Controllers.Payments
{
    [Route(Constants.ROUTE_TEMPLATE_CONTROLLER_ACTION)]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplePayMerchantValidator _merchantValidator;

        public PaymentController(ApplePayMerchantValidator applePayMerchantValidator)
        {
            _merchantValidator = applePayMerchantValidator;
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateAsync([FromBody] MerchantValidationRequest validation)
        {
            // You may wish to additionally validate that the URI specified for merchant validation in the
            // request body is a documented Apple Pay JS hostname. The IP addresses and DNS hostnames of
            // these servers are available here: https://developer.apple.com/documentation/applepayjs/setting_up_server_requirements
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var initiativeContext = Request.GetTypedHeaders().Host.Value; // the domain of your server which handling the payments.
            var sessionResponse = await _merchantValidator.ValidateAsync(validation.ValidationUrl, initiativeContext);

            var responsePayload = new MerchantValidationResponse
            {
                MerchantSession = sessionResponse
            };

            return Ok(responsePayload);
        }
    }
}
