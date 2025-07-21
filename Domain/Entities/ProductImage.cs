using Domain.Common;

namespace Domain.Entities;

public sealed class ProductImage : BaseAuditableEntity
{
    public Guid ProductId { get; private set; }
    public string Url { get; private set; } = default!;
    public bool IsMain { get; private set; }
    public int SortOrder { get; private set; }

    private ProductImage() { } // EF

    public ProductImage(Guid productId, string url, bool isMain, int sortOrder)
        => (ProductId, Url, IsMain, SortOrder) = (productId, url, isMain, sortOrder);
}
