using Domain.Enums;

namespace Application.Features.Products.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int StockQty { get; set; }
    public int CarbonGrams { get; set; }
    public EcoLabel Label { get; private set; }
    public List<string> Images { get; set; } = new();

}
