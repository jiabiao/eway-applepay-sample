using eWAY.Samples.MonkeyStore.PaymentGateway.DirectPayment;
using eWAY.Samples.MonkeyStore.PaymentGateway.TranparentRedirect;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eWAY.Samples.MonkeyStore.PaymentGateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IOptionsMonitor<PaymentGatewayOptions> optionsMonitor;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly JsonSerializerOptions serializerOptions;

        public PaymentService(IOptionsMonitor<PaymentGatewayOptions> optionsMonitor, IHttpClientFactory httpClientFactory)
        {
            this.optionsMonitor = optionsMonitor;
            this.httpClientFactory = httpClientFactory;

            serializerOptions = new JsonSerializerOptions
            {
                // PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
            serializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task<DirectPaymentResponse> PurchaseAsync(DirectPaymentRequest request)
        {
            return await DoRequestAsync<DirectPaymentRequest, DirectPaymentResponse>(request, PaymentGatewayOptions.PATH_DP_PAYMENT);
        }

        public async Task<GenerateAccessCodeResponse> CreateAccessTokenAsync(GenerateAccessCodeRequest request)
        {
            return await DoRequestAsync<GenerateAccessCodeRequest, GenerateAccessCodeResponse>(request, PaymentGatewayOptions.PATH_TR_ACCESS_CODE);
        }

        private async Task<TResponse> DoRequestAsync<TRequest, TResponse>(TRequest request, string resourcePath)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = BuildAuthorizationHeader();
            
            var jsonBody = JsonSerializer.Serialize(request, serializerOptions);
            var stringContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var endpoint = optionsMonitor.CurrentValue.Endpoint;
            var requestUri = $"{endpoint.TrimEnd('/')}/{resourcePath.TrimStart('/')}";

            var response = await httpClient.PostAsync(requestUri, stringContent);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TResponse>(responseJson);
        }

        private AuthenticationHeaderValue BuildAuthorizationHeader()
        {
            var account = $"{optionsMonitor.CurrentValue.Key}:{optionsMonitor.CurrentValue.Secret}";
            var bytes = Encoding.ASCII.GetBytes(account);
            var base64 = Convert.ToBase64String(bytes);
            return new AuthenticationHeaderValue("Basic", base64);
        }
    }
}
