using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : BaseAuditableEntity
    {
        private Order() { }

        public Order(Guid appUserId,
                     IEnumerable<OrderItem> items,
                     Address shipTo,
                     Money shippingFee)
        {
            AppUserId = appUserId;
            Items = items.ToList();
            ShippingAddress = shipTo;
            ShippingFee = shippingFee;
            Status = OrderStatus.Pending;
            Recalculate();
        }

        public Guid AppUserId { get; private set; }
        public AppUser AppUser { get; private set; } = default!;
        public OrderStatus Status { get; private set; }

        public Address ShippingAddress { get; private set; } = default!;
        public string? PaymentIntentId { get; private set; }

        public ICollection<OrderItem> Items { get; } = new List<OrderItem>();

        public Money SubTotal { get; private set; } = Money.Zero("INR");
        public Money ShippingFee { get; private set; }
        public Money Total => SubTotal + ShippingFee;

        private void Recalculate() =>
            SubTotal = Items.Select(i => i.SubTotal)
                            .Aggregate(Money.Zero("INR"), (a, b) => a + b);
        public void MarkAsPaid(string paymentIntentId)
        {
            // Idempotent: ignore if we processed this PI already
            if (Status == OrderStatus.Paid && PaymentIntentId == paymentIntentId) return;

            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Order is not in a payable state.");

            PaymentIntentId = paymentIntentId;
            Status = OrderStatus.Paid;
        }
        public void ChangeStatus(OrderStatus next)
        {
            if (Status == next) return;

            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cancelled orders cannot change state.");

            if (Status == OrderStatus.Shipped && next == OrderStatus.Pending)
                throw new InvalidOperationException("Cannot revert a shipped order to pending.");

            Status = next;
        }

    }
}
