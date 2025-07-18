using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Payments.Commands
{
    public class MarkOrderPaidHandler : IRequestHandler<MarkOrderPaidCommand>
    {
        private readonly IAppDbContext _db;
        public MarkOrderPaidHandler(IAppDbContext db) => _db = db;

        public async Task Handle(MarkOrderPaidCommand c, CancellationToken ct)
        {
            var order = await _db.Orders
                                 .FirstOrDefaultAsync(o => o.Id == c.OrderId, ct);

            if (order is null)                         
                return;

            order.MarkAsPaid(c.PaymentIntentId);       
            await _db.SaveChangesAsync(ct);
        }

    }

}
