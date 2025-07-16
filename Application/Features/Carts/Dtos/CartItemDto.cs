namespace Application.Features.Carts.Dtos;

public class CartItemDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }
}
