using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;
using System.Reflection.Emit;
namespace Domain.Entities;
public sealed class Product : BaseAuditableEntity
{
    private Product() { }

    public Product(string name,
                   Money price,
                   int stockQty,
                   int carbonGrams = 0,
                   EcoLabel label = EcoLabel.None)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (price.Amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be positive.");
        if (stockQty < 0) throw new ArgumentOutOfRangeException(nameof(stockQty));
        if (carbonGrams < 0) throw new ArgumentOutOfRangeException(nameof(carbonGrams));

        Sku = Ulid.NewUlid().ToString();        
        Name = name.Trim();
        Price = price;
        StockQty = stockQty;
        CarbonGrams = carbonGrams;
        Label = label;
    }

    public string Sku { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public Money Price { get; private set; }
    public int StockQty { get; private set; }
    public int CarbonGrams { get; private set; }
    public EcoLabel Label { get; private set; }

    public void AdjustStock(int delta)
    {
        var next = StockQty + delta;
        if (next < 0) throw new InvalidOperationException("Insufficient stock");
        StockQty = next;
        Touch();
    }

    public void SetEcoData(int carbonGrams, EcoLabel label)
    {
        if (carbonGrams < 0) throw new ArgumentOutOfRangeException(nameof(carbonGrams));
        CarbonGrams = carbonGrams;
        Label = label;
        Touch();
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice.Amount <= 0) throw new ArgumentOutOfRangeException(nameof(newPrice));
        Price = newPrice;
        Touch();
    }
}
