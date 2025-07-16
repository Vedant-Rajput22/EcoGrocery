using Application.Features.Carts.Dtos;
using MediatR;

namespace Application.Features.Carts.Commands;

public record RemoveItemFromCartCommand(
    Guid AppUserId,
    Guid ProductId) : IRequest<CartDto>;
