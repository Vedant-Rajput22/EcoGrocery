using Application.Features.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries
{
    public sealed record GetOrdersQuery(bool ForCurrentUserOnly = true)
        : IRequest<IEnumerable<OrderDto>>;
}
