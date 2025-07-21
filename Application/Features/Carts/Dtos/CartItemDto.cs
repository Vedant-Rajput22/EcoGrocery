namespace Application.Features.Carts.Dtos;

public class CartItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;  
    public decimal Price { get; set; }                    
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}
