using Application.Features.Carts.Dtos;
using MediatR;

namespace Application.Features.Carts.Commands;

public record AddItemToCartCommand(
    Guid AppUserId,
    Guid ProductId,
    int Quantity) : IRequest<CartDto>;
