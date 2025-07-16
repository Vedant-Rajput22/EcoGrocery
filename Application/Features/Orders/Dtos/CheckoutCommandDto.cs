using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Dtos
{
    public sealed record CheckoutDto(
    AddressDto Address,
    decimal ShippingFee);
}
