namespace Application.Features.Carts.Dtos;

public class CartDto
{
    public Guid Id { get; set; }
    public IEnumerable<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    public decimal SubTotal { get; set; }
}
