using Application.Features.Payments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Payments.Commands
{
    public sealed record CreatePaymentIntentCommand(Guid OrderId)
        : IRequest<PaymentIntentDto>;
}
