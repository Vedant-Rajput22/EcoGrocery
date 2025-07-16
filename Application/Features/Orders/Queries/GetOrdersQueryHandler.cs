using Application.Features.Orders.Dtos;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries
{
    public class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IAppDbContext _db;
        private readonly IMapper _map;
        private readonly IHttpContextAccessor _ctx;

        public GetOrdersQueryHandler(IAppDbContext db, IMapper map, IHttpContextAccessor ctx)
        {
            _db = db; _map = map; _ctx = ctx;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery q, CancellationToken ct)
        {
            var src = _db.Orders
                         .Include(o => o.Items)
                         .AsQueryable();

            if (q.ForCurrentUserOnly && _ctx.HttpContext!.User.IsInRole(BuiltInRoles.Customer))
            {
                var userId = Guid.Parse(_ctx.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                src = src.Where(o => o.AppUserId == userId);
            }

            return await src
                .OrderByDescending(o => o.CreatedUtc)
                .ProjectTo<OrderDto>(_map.ConfigurationProvider)
                .ToListAsync(ct);
        }
    }

}
