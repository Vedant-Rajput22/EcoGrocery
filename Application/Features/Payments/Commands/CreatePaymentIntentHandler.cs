using Application.Features.Payments.Commands;
using Application.Features.Payments.Dtos;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Application.Features.Payments.Handlers;

public sealed class CreatePaymentIntentHandler
    : IRequestHandler<CreatePaymentIntentCommand, PaymentIntentDto>
{
    private readonly IAppDbContext _db;
    public CreatePaymentIntentHandler(IAppDbContext db) => _db = db;

    public async Task<PaymentIntentDto> Handle(
        CreatePaymentIntentCommand c,
        CancellationToken ct)
    {
        var order = await _db.Orders
                             .Include(o => o.Items)
                             .FirstOrDefaultAsync(o => o.Id == c.OrderId, ct)
                    ?? throw new KeyNotFoundException("Order not found");

        // Stripe amount is in the smallest currency unit (paise for INR)
        var amountMinor = (long)Math.Round(order.Total.Amount * 100m);

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(new PaymentIntentCreateOptions
        {
            Amount = amountMinor,
            Currency = order.Total.Currency.ToLower(),
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            },
            Metadata = new Dictionary<string, string>
            {
                { "orderId", order.Id.ToString() }
            }
        }, cancellationToken: ct);

        return new PaymentIntentDto(intent.ClientSecret);
    }
}
