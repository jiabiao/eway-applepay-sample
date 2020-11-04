using eWAY.Samples.MonkeyStore.Models;
using eWAY.Samples.MonkeyStore.Repositories;
using eWAY.Samples.MonkeyStore.Web.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eWAY.Samples.MonkeyStore.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IAsyncRepository<Product> productRepository;

        public ProductController(IAsyncRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Product>> ListAsync()
        {
            return await productRepository.ListAllAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetAsync(int id)
        {
            var spec = new ProductSpecification(new[] { id });
            var product = await productRepository.FirstOrDefaultAsync(spec);

            if (product == null)
                return NotFound();

            return product;
        }
    }
}
