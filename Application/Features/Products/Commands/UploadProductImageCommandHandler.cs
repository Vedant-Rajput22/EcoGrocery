using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands;

/// <summary>
/// Saves each file to IFileStorageService and inserts ProductImage rows
/// without updating the Product itself (avoids concurrency conflicts).
/// </summary>
public sealed class UploadProductImagesCommandHandler
    : IRequestHandler<UploadProductImagesCommand, Unit>
{
    private readonly IAppDbContext _db;
    private readonly IFileStorageService _store;

    public UploadProductImagesCommandHandler(IAppDbContext db, IFileStorageService store)
        => (_db, _store) = (db, store);

    public async Task<Unit> Handle(UploadProductImagesCommand cmd, CancellationToken ct)
    {
        // 1️⃣  Make sure the product exists (no tracking → no concurrency tokens)
        var exists = await _db.Products
                              .AsNoTracking()
                              .AnyAsync(p => p.Id == cmd.ProductId, ct);
        if (!exists)
            throw new NotFoundException(nameof(Product), cmd.ProductId);

        // 2️⃣  Find current max SortOrder for this product
        var nextOrder = await _db.ProductImages
                                .Where(i => i.ProductId == cmd.ProductId)
                                .Select(i => (int?)i.SortOrder)
                                .MaxAsync(ct) ?? -1;
        nextOrder++;   // start at 0 if no images yet

        // 3️⃣  Build new ProductImage entities
        var newImages = new List<ProductImage>();
        foreach (var f in cmd.Files)
        {
            var url = await _store.UploadAsync(f.Bytes, f.FileName, f.ContentType, ct);
            newImages.Add(new ProductImage(
                cmd.ProductId,
                url,
                isMain: nextOrder == 0 && newImages.Count == 0,  // first ever image → main
                sortOrder: nextOrder++));
        }

        // 4️⃣  Insert rows; Product entity is untouched → no concurrency check
        _db.ProductImages.AddRange(newImages);
        await _db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
