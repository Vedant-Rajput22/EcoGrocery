using Application.Features.Carts.Dtos;
using MediatR;

namespace Application.Features.Carts.Queries;

public record GetActiveCartQuery(Guid AppUserId) : IRequest<CartDto>;
