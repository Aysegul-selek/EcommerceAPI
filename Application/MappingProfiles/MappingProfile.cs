using Application.Dtos.Category;
using Application.Dtos.Order;
using Application.Dtos.OrderItem;
using Application.Dtos.Product;
using Application.Dtos.RoleDto;
using Application.Dtos.UserDto;
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

            //Order
            CreateMap<Order, OrderDto>()
               .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Total))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            //OrderItem
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => ""));

            // Product mappingleri
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            // ProductReadDto mappingi: BaseEntity alanları dönmesin
            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            CreateMap<User, UserReadDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
            src.UserRoles.Select(ur => ur.Role)));

            CreateMap<Role, RoleReadDto>();

            CreateMap<Role, RoleReadDto>()
            .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.UserRoles.Count));

            CreateMap<ProductImage, ProductImageDto>().ReverseMap();
            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        }
    }
}
