// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.Options;
using MonkeyStore.PaymentGateway.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace MonkeyStore.PaymentGateway.Services
{
    public class DefaultQueryService : IQueryService
    {
        private readonly GatewayAuthenticationBuilder _authBuilder;
        private readonly IOptionsMonitor<GatewayEndpoints> _optionsMonitor;
        private readonly IHttpClientFactory _httpClientFactory;
        public DefaultQueryService(GatewayAuthenticationBuilder authBuilder,
                                   IOptionsMonitor<GatewayEndpoints> optionsMonitor,
                                   IHttpClientFactory httpClientFactory)
        {
            _authBuilder = authBuilder;
            _optionsMonitor = optionsMonitor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetByAccessCodeAsync(string accessCode)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = _authBuilder.Build();

            var endpoint = _optionsMonitor.CurrentValue.DirectPayment; // The query endpoint is the same for all API.
            var response = await httpClient.GetAsync($"{endpoint.TrimEnd('/')}/AccessCode");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetByTransactionId(string id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = _authBuilder.Build();

            var endpoint = _optionsMonitor.CurrentValue.DirectPayment; // The query endpoint is the same for all API.
            var response = await httpClient.GetAsync($"{endpoint.TrimEnd('/')}/Transactions/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
