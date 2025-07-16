using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Dtos
{
    public sealed record RegisterDto(
    string Email,
    string Password,
    string FirstName,
    string LastName);
}
