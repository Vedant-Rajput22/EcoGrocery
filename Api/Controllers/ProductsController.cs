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
        [HttpPost("{id:guid}/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImages(
        Guid id,
        [FromForm] IFormFileCollection files,
        [FromServices] ISender sender)               // ISender is already injected
        {
            if (files.Count == 0) return BadRequest("No files supplied.");

            var list = new List<FileUploadVm>();         // ← same namespace as the command
            foreach (var f in files)
            {
                if (!f.ContentType.StartsWith("image/"))
                    return BadRequest($"Not an image: {f.FileName}");

                await using var ms = new MemoryStream();
                await f.CopyToAsync(ms);
                list.Add(new FileUploadVm(ms.ToArray(), f.FileName, f.ContentType));
            }

            await sender.Send(new UploadProductImagesCommand(id, list.AsReadOnly())); // IReadOnlyList
            return Ok();
        }


    }

}
