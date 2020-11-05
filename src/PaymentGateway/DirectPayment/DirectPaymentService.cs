// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyStore.PaymentGateway.DirectPayment.Messages;
using MonkeyStore.PaymentGateway.Options;
using MonkeyStore.PaymentGateway.Services;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonkeyStore.PaymentGateway.DirectPayment
{
    public class DirectPaymentService : IPurchaseService<DirectPaymentRequest, DirectPaymentResponse>
    {
        private readonly GatewayAuthenticationBuilder _authBuilder;
        private readonly IOptionsMonitor<GatewayEndpoints> _optionsMonitor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<DirectPaymentService> _logger;

        public DirectPaymentService(GatewayAuthenticationBuilder authBuilder,
                                    IOptionsMonitor<GatewayEndpoints> optionsMonitor,
                                    IHttpClientFactory httpClientFactory,
                                    ILogger<DirectPaymentService> logger)
        {
            _authBuilder = authBuilder;
            _optionsMonitor = optionsMonitor;
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            _serializerOptions = new JsonSerializerOptions
            {
                // PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
            _serializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task<DirectPaymentResponse> PurchaseAsync(DirectPaymentRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = _authBuilder.Build();
            var jsonBody = JsonSerializer.Serialize(request, _serializerOptions);
            var stringContent = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);

            var endpoint = _optionsMonitor.CurrentValue.DirectPayment;
            var requestUri = $"{endpoint.TrimEnd('/')}/Transactions";

            var response = await httpClient.PostAsync(requestUri, stringContent);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<DirectPaymentResponse>(json);
        }
    }
}
