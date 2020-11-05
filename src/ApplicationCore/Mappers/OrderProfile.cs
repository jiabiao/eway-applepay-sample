// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using AutoMapper;
using MonkeyStore.DTOs;
using MonkeyStore.Models;

namespace MonkeyStore.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
        }
    }
}
