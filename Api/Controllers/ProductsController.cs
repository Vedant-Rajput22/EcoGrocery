using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ISender _sender;
        public ProductsController(ISender sender) => _sender = sender;
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetProductsQuery q)
            => Ok(await _sender.Send(q));
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand cmd)
        {
            var id = await _sender.Send(cmd);
            return CreatedAtAction(nameof(Get), new { id }, null);
        }
    }
}
