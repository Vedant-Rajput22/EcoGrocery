using Application.Features.Orders.Commands;
using Application.Features.Orders.Dtos;
using Application.Features.Orders.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]                    
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;
    public OrdersController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
        => Ok(await _sender.Send(new GetOrdersQuery()));

    [HttpGet("all")]
    [Authorize(Roles = BuiltInRoles.Admin)]
    public async Task<IActionResult> GetAllOrders()
        => Ok(await _sender.Send(new GetOrdersQuery(false)));

    [HttpPost("checkout/{cartId:guid}")]
    [Authorize(Roles = BuiltInRoles.Customer)]
    public async Task<IActionResult> Checkout(Guid cartId,
                                              [FromBody] CheckoutDto body)
    {
        var id = await _sender.Send(new PlaceOrderCommand(
                     cartId,
                     body.Address,
                     body.ShippingFee));
        return CreatedAtAction(nameof(GetMyOrders), new { id }, null);
    }
}
