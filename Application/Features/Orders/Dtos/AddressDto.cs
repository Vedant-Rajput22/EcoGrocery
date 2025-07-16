using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Dtos
{
    public sealed record AddressDto(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country);
}
