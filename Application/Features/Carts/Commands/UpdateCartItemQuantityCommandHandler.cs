using Application.Features.Carts.Dtos;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Features.Carts.Commands;

public class UpdateCartItemQuantityCommandHandler
    : IRequestHandler<UpdateCartItemQuantityCommand, CartDto>
{
    private readonly IAppDbContext _db;
    private readonly IMapper _map;

    public UpdateCartItemQuantityCommandHandler(IAppDbContext db, IMapper map)
    {
        _db = db;
        _map = map;
    }

    public async Task<CartDto> Handle(UpdateCartItemQuantityCommand c, CancellationToken ct)
    {
        if (c.Quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(c.Quantity));

        var cart = await _db.Carts
                            .FirstOrDefaultAsync(x => x.AppUserId == c.AppUserId && x.IsActive, ct)
                   ?? throw new KeyNotFoundException("Active cart not found");

        var item = await _db.CartItems
                            .FirstOrDefaultAsync(i => i.CartId == cart.Id &&
                                                      i.ProductId == c.ProductId, ct)
                   ?? throw new KeyNotFoundException("Item not found in cart");

        item.Decrease(item.Quantity);
        item.Increase(c.Quantity);

        await _db.SaveChangesAsync(ct);

        return await _db.Carts
                        .Where(x => x.Id == cart.Id)
                        .Include(x => x.Items).ThenInclude(i => i.Product)
                        .ProjectTo<CartDto>(_map.ConfigurationProvider)
                        .SingleAsync(ct);
    }
}
