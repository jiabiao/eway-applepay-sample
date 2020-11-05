// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.Options;
using MonkeyStore.PaymentGateway.Messages;
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
    public class TransparentRedirectService : IAccessCodeProvider
    {
        private readonly GatewayAuthenticationBuilder _authBuilder;
        private readonly IOptionsMonitor<GatewayEndpoints> _optionsMonitor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _serializerOptions;

        public TransparentRedirectService(GatewayAuthenticationBuilder authBuilder,
                                         IOptionsMonitor<GatewayEndpoints> optionsMonitor,
                                         IHttpClientFactory httpClientFactory)
        {
            _authBuilder = authBuilder;
            _optionsMonitor = optionsMonitor;
            _httpClientFactory = httpClientFactory;

            _serializerOptions = new JsonSerializerOptions
            {
                // PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
            _serializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task<CreateAccessCodeResponse> CreateAccessCodeAsync(CreateAccessCodeRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = _authBuilder.Build();

            var jsonBody = JsonSerializer.Serialize(request, _serializerOptions);
            var stringContent = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);

            var endpoint = _optionsMonitor.CurrentValue.TransparentRedirect;
            var requestUri = $"{endpoint.TrimEnd('/')}/AccessCodes";

            var response = await httpClient.PostAsync(requestUri, stringContent);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CreateAccessCodeResponse>(json);
        }
    }
}
