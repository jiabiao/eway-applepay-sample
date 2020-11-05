// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonkeyStore.Models;
using MonkeyStore.Repositories;
using MonkeyStore.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonkeyStore.Controllers
{
    [Route(Constants.ROUTE_TEMPLATE_CONTROLLER)]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IAsyncRepository<Product> _productRepository;

        public ProductController(IAsyncRepository<Product> productRepository)
        {
            this._productRepository = productRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Product>> ListAsync()
        {
            return await _productRepository.ListAllAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetAsync(int id)
        {
            var spec = new ProductSpecification(new[] { id });
            var product = await _productRepository.FirstOrDefaultAsync(spec);

            if (product == null)
                return NotFound();

            return product;
        }
    }
}
