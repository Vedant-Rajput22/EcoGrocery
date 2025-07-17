using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        private OrderItem() { }

        public OrderItem(Guid productId, int qty, Money unitPrice)
        {
            ProductId = productId;
            Quantity = qty;
            UnitPrice = unitPrice;
        }

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;

        public Guid OrderId { get; private set; }
        public Order Order { get; private set; } = default!;

        public int Quantity { get; private set; }
        public Money UnitPrice { get; private set; }
        public Money SubTotal => UnitPrice * Quantity;
    }
}
    