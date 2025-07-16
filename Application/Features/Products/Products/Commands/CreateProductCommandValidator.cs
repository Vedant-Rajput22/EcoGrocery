using Domain.Enums;
using FluentValidation;

namespace Application.Features.Products.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.StockQty).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CarbonGrams).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Label)
        .IsInEnum()                     
        .WithMessage("Unsupported eco-label.");

    }
}
