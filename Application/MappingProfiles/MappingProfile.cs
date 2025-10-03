using Application.Dtos.Category;
using Application.Dtos.Order;
using Application.Dtos.OrderItem;
using Application.Dtos.Product;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category mappingleri 
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();

            // Order -> OrderDto
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            // OrderItem -> OrderItemDto
            CreateMap<OrderItem, OrderItemDto>();

            // CreateOrderDto -> Order
            CreateMap<CreateOrderDto, Order>();

            // CreateOrderItemDto -> OrderItem
            CreateMap<CreateOrderItemDto, OrderItem>();

            // Product mappingleri
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
        }
    }
}
