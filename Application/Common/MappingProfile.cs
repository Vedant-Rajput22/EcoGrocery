using Application.Features.Carts.Dtos;
using Application.Features.Orders.Dtos;
using Application.Features.Products.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Address, AddressDto>();
        CreateMap<CartItem, CartItemDto>()
            .ForMember(d => d.Name,
                       o => o.MapFrom(s => s.Product!.Name))
            .ForMember(d => d.LineTotal,
                       o => o.MapFrom(s => s.UnitPrice.Amount * s.Quantity));

        CreateMap<Cart, CartDto>()
            .ForMember(d => d.SubTotal,
                       o => o.MapFrom(s => s.SubTotal.Amount));

        CreateMap<Product, ProductDto>()
            .ForMember(d => d.Price,
                       o => o.MapFrom(s => s.Price.Amount))
            .ForMember(d => d.CarbonGrams,
                        o => o.MapFrom(s => s.CarbonGrams))
                            .ForMember(d => d.Label,
                        o => o.MapFrom(s => s.Label));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.Name,
                          o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.UnitPrice,
               o => o.MapFrom(s => s.UnitPrice.Amount))
            .ForMember(d => d.LineTotal,
               o => o.MapFrom(s => s.UnitPrice.Amount * s.Quantity));

        //CreateMap<Order, OrderDto>()
        //    .ForMember(d => d.SubTotal,
        //               o => o.MapFrom(s => s.SubTotal.Amount))
        //    .ForMember(d => d.ShippingFee,
        //               o => o.MapFrom(s => s.ShippingFee.Amount))
        //    .ForMember(d => d.Total,
        //               o => o.MapFrom(s => s.Total.Amount))
        //    .ForMember(d => d.Status,
        //               o => o.MapFrom(s => s.Status.ToString()))
        //    .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedUtc));

        CreateMap<Order, OrderDto>()
            .ForMember(d => d.CustomerName,
                       o => o.MapFrom(s => s.AppUser.FirstName + " " + s.AppUser.LastName))
            .ForMember(d => d.ShippingAddress,
                       o => o.MapFrom(s => s.ShippingAddress))          
            .ForMember(d => d.SubTotal,
                       o => o.MapFrom(s => s.SubTotal.Amount))
            .ForMember(d => d.ShippingFee,
                       o => o.MapFrom(s => s.ShippingFee.Amount))
            .ForMember(d => d.Total,
                       o => o.MapFrom(s => s.Total.Amount))
            .ForMember(d => d.CreatedOn,
                       o => o.MapFrom(s => s.CreatedUtc))
            .ForMember(d => d.Status,
                       o => o.MapFrom(s => s.Status.ToString()));
    }
}
