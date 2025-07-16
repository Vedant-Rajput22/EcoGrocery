using Application.Features.Carts.Dtos;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts.Commands;

public class AddItemToCartCommandHandler
    : IRequestHandler<AddItemToCartCommand, CartDto>
{
    private readonly IAppDbContext _db;
    private readonly IMapper _map;

    public AddItemToCartCommandHandler(IAppDbContext db, IMapper map)
    {
        _db = db;
        _map = map;
    }

    public async Task<CartDto> Handle(AddItemToCartCommand c, CancellationToken ct)
    {
        var product = await _db.Products
                               .SingleOrDefaultAsync(p => p.Id == c.ProductId, ct)
                      ?? throw new KeyNotFoundException("Product not found");

        var cart = await _db.Carts
                            .FirstOrDefaultAsync(x => x.AppUserId == c.AppUserId && x.IsActive, ct)
                   ?? new Cart(c.AppUserId);

        if (_db.Carts.Entry(cart).State == EntityState.Detached)
            _db.Carts.Add(cart);

        var item = await _db.CartItems
                            .FirstOrDefaultAsync(i => i.CartId == cart.Id &&
                                                      i.ProductId == product.Id, ct);

        if (item is null)
            _db.CartItems.Add(new CartItem(cart.Id, product.Id, c.Quantity,
                                           new Money(product.Price.Amount, product.Price.Currency)));
        else
            item.Increase(c.Quantity);

        await _db.SaveChangesAsync(ct);

        return await _db.Carts
                        .Where(x => x.Id == cart.Id)
                        .Include(x => x.Items).ThenInclude(i => i.Product)
                        .ProjectTo<CartDto>(_map.ConfigurationProvider)
                        .SingleAsync(ct);
    }
}
