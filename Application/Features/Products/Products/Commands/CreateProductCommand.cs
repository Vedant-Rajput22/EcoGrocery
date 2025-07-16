using Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Products.Commands;

public record CreateProductCommand(
    [Required] string Name,
    [Required] decimal Price,
    [Required] int StockQty,
    int CarbonGrams = 0,
    EcoLabel Label = EcoLabel.None
) : IRequest<Guid>;
