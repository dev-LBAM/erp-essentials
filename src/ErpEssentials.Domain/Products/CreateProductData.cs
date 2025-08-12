namespace ErpEssentials.Domain.Products;

public record CreateProductData(
    string Sku,
    string Name,
    string? Description,
    string? Barcode,
    decimal Price,
    decimal Cost,
    Guid BrandId,
    Guid CategoryId);