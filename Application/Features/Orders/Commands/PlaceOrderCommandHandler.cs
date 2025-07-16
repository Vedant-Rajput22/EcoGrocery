using Application.Features.Orders.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands;

public class PlaceOrderCommandHandler
    : IRequestHandler<PlaceOrderCommand, Guid>
{
    private readonly IAppDbContext _db;
    public PlaceOrderCommandHandler(IAppDbContext db) => _db = db;

    public async Task<Guid> Handle(PlaceOrderCommand c, CancellationToken ct)
    {
        
        var cart = await _db.Carts
                            .Include(x => x.Items)
                            .ThenInclude(i => i.Product)
                            .FirstOrDefaultAsync(x => x.Id == c.CartId, ct)
                   ?? throw new KeyNotFoundException("Cart not found");

        if (!cart.Items.Any())
            throw new InvalidOperationException("Cart is empty");

        var items = cart.Items.Select(i =>
            new OrderItem(
                i.ProductId,
                i.Quantity,
                new Money(i.UnitPrice.Amount, i.UnitPrice.Currency)))
            .ToList();

        var shipTo = new Address(
            c.ShippingAddress.Street,
            c.ShippingAddress.City,
            c.ShippingAddress.State,
            c.ShippingAddress.PostalCode,
            c.ShippingAddress.Country);

        var shipFee = new Money(c.ShippingFee, "INR");

        var order = new Order(cart.AppUserId, items, shipTo, shipFee);

        _db.Orders.Add(order);
        _db.Carts.Remove(cart);
        await _db.SaveChangesAsync(ct);

        return order.Id;
    }
}
