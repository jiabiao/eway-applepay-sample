// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.Options;
using MonkeyStore.PaymentGateway.Messages;
using MonkeyStore.PaymentGateway.Options;
using MonkeyStore.PaymentGateway.Services;
using MonkeyStore.PaymentGateway.SharedPage.Messages;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonkeyStore.PaymentGateway.SharedPage
{
    public class SharedPageService : IAccessCodeSharedProvider<CreateAccessCodeSharedResponse>
    {
        private readonly GatewayAuthenticationBuilder _authBuilder;
        private readonly IOptionsMonitor<GatewayEndpoints> _optionsMonitor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _serializerOptions;

        public SharedPageService(GatewayAuthenticationBuilder authBuilder,
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

        public async Task<CreateAccessCodeSharedResponse> CreateAccessCodeSharedAsync(CreateAccessCodeRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = _authBuilder.Build();

            var jsonBody = JsonSerializer.Serialize(request, _serializerOptions);
            var stringContent = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);

            var endpoint = _optionsMonitor.CurrentValue.ResponsiveSharedPage;
            var requestUri = $"{endpoint.TrimEnd('/')}/AccessCodesShared";

            var response = await httpClient.PostAsync(requestUri, stringContent);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CreateAccessCodeSharedResponse>(json);
        }
    }
}
