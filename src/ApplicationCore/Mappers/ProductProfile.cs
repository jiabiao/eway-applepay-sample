// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using AutoMapper;
using MonkeyStore.DTOs;
using MonkeyStore.Models;

namespace MonkeyStore.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
