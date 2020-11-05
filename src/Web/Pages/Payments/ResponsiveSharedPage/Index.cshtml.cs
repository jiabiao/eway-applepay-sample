// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonkeyStore.DTOs;
using MonkeyStore.Models;
using MonkeyStore.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonkeyStore.Pages.Payments.ResponsiveSharedPage
{
    public class IndexModel : PageModel
    {
        private readonly IAsyncRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public IReadOnlyList<ProductDto> Products { get; set; }

        public IndexModel(IAsyncRepository<Product> productRepository,
                          IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task OnGetAsync()
        {
            var products = await _productRepository.ListAllAsync();
            Products = _mapper.Map<List<ProductDto>>(products);
        }
    }
}
