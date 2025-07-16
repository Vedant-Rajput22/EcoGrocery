using Api.Extensions;
using Application.Features.Carts.Commands;
using Application.Features.Carts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ISender _sender;
        public CartController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var dto = await _sender.Send(new GetActiveCartQuery(userId));
            return Ok(dto);
        }

        [HttpPost("items")]
        public async Task<IActionResult> Add(AddItemToCartCommand body)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var dto = await _sender.Send(body with { AppUserId = userId });
            return Ok(dto);
        }
        [HttpPut("items/{productId:guid}")]
        public async Task<IActionResult> Update(Guid productId, [FromBody] int quantity)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var dto = await _sender.Send(new UpdateCartItemQuantityCommand(
                            userId, productId, quantity));

            return Ok(dto);
        }
        [HttpDelete("items/{productId:guid}")]
        public async Task<IActionResult> Delete(Guid productId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var dto = await _sender.Send(new RemoveItemFromCartCommand(
                            userId, productId));

            return Ok(dto);
        }
    }
}
