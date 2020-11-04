using eWAY.Samples.MonkeyStore.PaymentGateway.DirectPayment;
using eWAY.Samples.MonkeyStore.PaymentGateway.TranparentRedirect;
using System.Threading.Tasks;

namespace eWAY.Samples.MonkeyStore.PaymentGateway.Services
{
    public interface IPaymentService
    {
        Task<DirectPaymentResponse> PurchaseAsync(DirectPaymentRequest request);

        Task<GenerateAccessCodeResponse> CreateAccessTokenAsync(GenerateAccessCodeRequest request);
    }
}
