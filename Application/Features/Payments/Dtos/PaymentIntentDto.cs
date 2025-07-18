using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Payments.Dtos
{
    public sealed record PaymentIntentDto(string ClientSecret);
}
