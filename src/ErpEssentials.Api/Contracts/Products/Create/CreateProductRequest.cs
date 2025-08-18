namespace ErpEssentials.Api.Contracts.Products.Create;

public class CreateProductRequest
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }
}