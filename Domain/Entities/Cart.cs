using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Cart : BaseAuditableEntity
{
    private Cart() { }

    public Cart(Guid appUserId) { AppUserId = appUserId; IsActive = true; }

    public Guid AppUserId { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<CartItem> Items { get; } = new List<CartItem>();

    public Money SubTotal =>
        Items.Select(i => i.UnitPrice * i.Quantity)
             .DefaultIfEmpty(Money.Zero("INR"))
             .Aggregate((a, b) => a + b);
}
