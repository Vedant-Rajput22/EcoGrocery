using Application.Features.Carts.Dtos;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Features.Carts.Commands;

public class RemoveItemFromCartCommandHandler
    : IRequestHandler<RemoveItemFromCartCommand, CartDto>
{
    private readonly IAppDbContext _db;
    private readonly IMapper _map;

    public RemoveItemFromCartCommandHandler(IAppDbContext db, IMapper map)
    {
        _db = db;
        _map = map;
    }

    public async Task<CartDto> Handle(RemoveItemFromCartCommand c, CancellationToken ct)
    {
        var cart = await _db.Carts
                            .FirstOrDefaultAsync(x => x.AppUserId == c.AppUserId && x.IsActive, ct)
                   ?? throw new KeyNotFoundException("Active cart not found");

        var item = await _db.CartItems
                            .FirstOrDefaultAsync(i => i.CartId == cart.Id &&
                                                      i.ProductId == c.ProductId, ct)
                   ?? throw new KeyNotFoundException("Item not found in cart");

        _db.CartItems.Remove(item);

        await _db.SaveChangesAsync(ct);

        return await _db.Carts
                        .Where(x => x.Id == cart.Id)
                        .Include(x => x.Items).ThenInclude(i => i.Product)
                        .ProjectTo<CartDto>(_map.ConfigurationProvider)
                        .SingleAsync(ct);
    }
}
