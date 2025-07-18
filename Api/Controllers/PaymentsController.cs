using System.IO;
using Application.Features.Payments.Commands;
using Application.Features.Payments.Dtos;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IConfiguration _cfg;

    public PaymentsController(ISender sender, IConfiguration cfg)
    {
        _sender = sender;
        _cfg = cfg;
    }

    // POST /api/payments/intent/{orderId}
    [HttpPost("intent/{orderId:guid}")]
    [Authorize(Roles = BuiltInRoles.Customer)]
    public async Task<ActionResult<PaymentIntentDto>> CreateIntent(Guid orderId)
    {
        var dto = await _sender.Send(new CreatePaymentIntentCommand(orderId));
        return Ok(dto);
    }

    // POST /api/payments/webhook    ← called by Stripe
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebhook()
    {
        var payload = await new StreamReader(Request.Body).ReadToEndAsync();
        var sigHeader = Request.Headers["Stripe-Signature"];
        var hookKey = _cfg["Stripe:WebhookKey"];

        Event evt;
        try
        {
            evt = EventUtility.ConstructEvent(payload, sigHeader, hookKey);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️  Webhook signature check failed: {ex.Message}");
            return BadRequest();
        }

        if (evt.Type == "payment_intent.succeeded")
        {
            var intent = (PaymentIntent)evt.Data.Object;

            // Ensure we stored orderId as metadata when creating the PI
            if (intent.Metadata.TryGetValue("orderId", out var rawId) &&
                Guid.TryParse(rawId, out var orderId))
            {
                await _sender.Send(new MarkOrderPaidCommand(orderId, intent.Id));
            }
            else
            {
                Console.WriteLine("⚠️  PaymentIntent lacks orderId metadata");
            }
        }

        // Stripe only needs a 2xx to stop retrying
        return Ok();
    }
}
