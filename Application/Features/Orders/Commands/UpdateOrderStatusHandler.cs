using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly IAppDbContext _db;
    public UpdateOrderStatusHandler(IAppDbContext db) => _db = db;

    public async Task Handle(UpdateOrderStatusCommand c, CancellationToken ct)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == c.OrderId, ct)
                    ?? throw new KeyNotFoundException("Order not found");

        order.ChangeStatus(c.NewStatus);
        await _db.SaveChangesAsync(ct);
    }
}
