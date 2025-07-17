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
    public void AddItem(Product product, int qty)
    {
        if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
        if (qty > product.StockQty)
            throw new InvalidOperationException("Requested quantity exceeds stock.");

        var line = Items.FirstOrDefault(i => i.ProductId == product.Id);

        if (line is null)
        {
            var priceCopy = new Money(product.Price.Amount, product.Price.Currency);
            Items.Add(new CartItem(this.Id, product.Id, qty, priceCopy));   
        }
        else
        {
            var newQty = line.Quantity + qty;
            if (newQty > product.StockQty)
                throw new InvalidOperationException("Requested quantity exceeds stock.");
            line.Increase(qty);                                             
        }
    }
}
