// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using MonkeyStore.PaymentGateway.Constants;
using MonkeyStore.PaymentGateway.Messages;
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
        private readonly IUriComposer _uriComposer;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _serializerOptions;

        public SharedPageService(GatewayAuthenticationBuilder authBuilder,
                                 IUriComposer uriComposer,
                                 IHttpClientFactory httpClientFactory)
        {
            _authBuilder = authBuilder;
            _uriComposer = uriComposer;
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

            var requestUri = _uriComposer.Build(ResourceTemplates.TEMPLATE_ACCESS_CODES_SHARED);
            var response = await httpClient.PostAsync(requestUri, stringContent);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CreateAccessCodeSharedResponse>(json);
        }
    }
}
