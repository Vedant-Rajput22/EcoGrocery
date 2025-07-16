using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class CartItem : BaseEntity
{
    private CartItem() { }

    public CartItem(Guid cartId, Guid productId, int qty, Money price)
    {
        CartId = cartId;
        ProductId = productId;
        Quantity = qty;
        UnitPrice = price;
    }

    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }

    public Product? Product { get; private set; }  

    public void Increase(int q) => Quantity += q;
    public void Decrease(int q) => Quantity = Math.Max(0, Quantity - q);
}
