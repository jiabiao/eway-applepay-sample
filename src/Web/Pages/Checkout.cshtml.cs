using eWAY.Samples.MonkeyStore.Models;
using eWAY.Samples.MonkeyStore.PaymentGateway;
using eWAY.Samples.MonkeyStore.Repositories;
using eWAY.Samples.MonkeyStore.Web.Specifications;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace eWAY.Samples.MonkeyStore.Web.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly IAsyncRepository<Product> productRepository;
        private readonly IOptionsMonitor<PaymentGatewayOptions> paymentGatewayOptions;

        public Product Product { get; set; }

        public string  PayUrl { get; set; }


        public CheckoutModel(IAsyncRepository<Product> productRepository
                           , IOptionsMonitor<PaymentGatewayOptions> paymentGatewayOptions)
        {
            this.productRepository = productRepository;
            this.paymentGatewayOptions = paymentGatewayOptions;
        }

        public async Task OnGetAsync(int id)
        {
            var spec = new ProductSpecification(new[] { id });
            Product =await productRepository.FirstOrDefaultAsync(spec);
            PayUrl = $"{paymentGatewayOptions.CurrentValue.Endpoint.TrimEnd('/')}/{PaymentGateway.PaymentGatewayOptions.PATH_TR_ACCESS_CODE.TrimStart('/')}";
        }
    }
}
