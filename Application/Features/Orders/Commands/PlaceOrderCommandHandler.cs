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
        using var tx = await _db.BeginTransactionAsync(ct);

        var cart = await _db.Carts
                            .Include(x => x.Items).ThenInclude(i => i.Product)
                            .FirstOrDefaultAsync(x => x.Id == c.CartId, ct)
                   ?? throw new KeyNotFoundException("Cart not found");

        if (!cart.Items.Any())
            throw new InvalidOperationException("Cart is empty");

        var items = new List<OrderItem>();

        foreach (var line in cart.Items)
        {
            if (line.Quantity > line.Product!.StockQty)
                throw new InvalidOperationException($"Insufficient stock for {line.Product.Name}");

            line.Product.AdjustStock(-line.Quantity);           

            items.Add(new OrderItem(
                line.ProductId,
                line.Quantity,
                new Money(line.UnitPrice.Amount, line.UnitPrice.Currency)));
        }

        var shipTo = new Address(
            c.ShippingAddress.Street,
            c.ShippingAddress.City,
            c.ShippingAddress.State,
            c.ShippingAddress.PostalCode,
            c.ShippingAddress.Country);

        var fee = new Money(c.ShippingFee, "INR");

        var order = new Order(cart.AppUserId, items, shipTo, fee);

        _db.Orders.Add(order);
        _db.Carts.Remove(cart);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return order.Id;
    }

}
