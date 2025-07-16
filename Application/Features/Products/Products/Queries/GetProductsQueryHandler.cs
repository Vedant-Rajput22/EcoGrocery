using Application.Features.Products.Dtos;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries;

public class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IAppDbContext _db;
    private readonly IMapper _map;

    public GetProductsQueryHandler(IAppDbContext db, IMapper map)
    {
        _db = db;
        _map = map;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery q, CancellationToken ct)
    {
        var src = _db.Products.AsQueryable();

        if (q.LabelFilter is not null)
            src = src.Where(p => p.Label == q.LabelFilter.Value);

        if (q.MaxCarbonGrams is not null)
            src = src.Where(p => p.CarbonGrams <= q.MaxCarbonGrams);

        return await src.OrderBy(p => p.Name)
                        .ProjectTo<ProductDto>(_map.ConfigurationProvider)
                        .ToListAsync(ct);
    }
}
