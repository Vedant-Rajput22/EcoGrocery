using Application.Features.Products.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Features.Products.Queries;

public record GetProductsQuery(EcoLabel? LabelFilter, int? MaxCarbonGrams)
     : IRequest<IEnumerable<ProductDto>>;