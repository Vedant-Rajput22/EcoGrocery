using Application.Features.Carts.Dtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts.Queries;

public class GetActiveCartQueryHandler
    : IRequestHandler<GetActiveCartQuery, CartDto>
{
    private readonly IAppDbContext _db;
    private readonly IMapper _map;

    public GetActiveCartQueryHandler(IAppDbContext db, IMapper map)
    {
        _db = db;
        _map = map;
    }

    public async Task<CartDto> Handle(GetActiveCartQuery q, CancellationToken ct)
    {
        var cart = await _db.Carts
                            .Include(c => c.Items).ThenInclude(i => i.Product)
                            .FirstOrDefaultAsync(c => c.AppUserId == q.AppUserId && c.IsActive, ct)
                   ?? new Domain.Entities.Cart(q.AppUserId);

        return _map.Map<CartDto>(cart);
    }
}
