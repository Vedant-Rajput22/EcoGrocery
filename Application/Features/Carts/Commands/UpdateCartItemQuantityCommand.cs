using Application.Features.Carts.Dtos;
using MediatR;

namespace Application.Features.Carts.Commands;

public record UpdateCartItemQuantityCommand(
    Guid AppUserId,
    Guid ProductId,
    int Quantity) : IRequest<CartDto>;
