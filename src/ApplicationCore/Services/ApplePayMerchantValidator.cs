// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.Logging;
using MonkeyStore.DTOs.ApplePay;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonkeyStore.Services
{
    public class ApplePayMerchantValidator
    {
        private readonly ApplePayCertificateProvider _certificateProvider;
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<ApplePayMerchantValidator> _logger;

        public ApplePayMerchantValidator(ApplePayCertificateProvider certificateProvider,
                                         IHttpClientFactory clientFactory,
                                         ILogger<ApplePayMerchantValidator> logger)
        {
            _certificateProvider = certificateProvider;
            _clientFactory = clientFactory;
            _logger = logger;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        public async Task<MerchantSessionResponose> ValidateAsync(string validationUrl, string initiativeContext)
        {
            var identifider = _certificateProvider.GetMerchantIdentifier();

            // Create the JSON payload to POST to the Apple Pay merchant validation URL.
            var request = new MerchantSessionRequest()
            {
                DisplayName = "Monkey Store",
                Initiative = "web",
                InitiativeContext = initiativeContext,
                MerchantIdentifier = identifider
            };

            var client = _clientFactory.CreateClient(Constants.NamedHttpClientApplePay);

            var jsonContent = JsonSerializer.Serialize(request, _jsonOptions);

            var requestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(validationUrl),
                Method = HttpMethod.Post,
                Content = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            _logger.LogInformation("start to merchant validation.");
            var response = await client.SendAsync(requestMessage);

            _logger.LogInformation("merchant validation responsed from Apple.");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var merchantSession = JsonSerializer.Deserialize<MerchantSessionResponose>(json, _jsonOptions);
            return merchantSession;
        }
    }
}
