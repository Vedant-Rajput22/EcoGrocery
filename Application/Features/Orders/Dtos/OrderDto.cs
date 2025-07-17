using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Dtos
{
    public sealed class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal SubTotal { get; set; }
        public string CustomerName { get; set; } = default!;      
        public AddressDto ShippingAddress { get; set; } = default!;  
        public decimal ShippingFee { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = default!;
        public List<OrderItemDto> Items { get; set; } = [];
    }
}
