using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Products.Commands;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IAppDbContext _db;
    public CreateProductCommandHandler(IAppDbContext db) => _db = db;

    public async Task<Guid> Handle(CreateProductCommand c, CancellationToken ct)
    {
        var product = new Product(
            c.Name,
            new Money(c.Price, "INR"),
            c.StockQty,
            c.CarbonGrams,
            c.Label);

        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);
        return product.Id;
    }

}
