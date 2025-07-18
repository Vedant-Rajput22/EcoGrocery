using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Payments.Commands
{
    public sealed record MarkOrderPaidCommand(Guid OrderId, string PaymentIntentId) : IRequest;

}
