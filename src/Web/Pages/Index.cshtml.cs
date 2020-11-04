using eWAY.Samples.MonkeyStore.Models;
using eWAY.Samples.MonkeyStore.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eWAY.Samples.MonkeyStore.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAsyncRepository<Product> productRepository;

        public IReadOnlyList<Product> Products { get; set; }

        public IndexModel(IAsyncRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task OnGetAsync()
        {
            Products =await productRepository.ListAllAsync();
        }
    }
}
