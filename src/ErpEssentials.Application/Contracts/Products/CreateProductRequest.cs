namespace ErpEssentials.Application.Contracts.Products;

public record CreateProductRequest
(
     string Sku,
     string Name,
     string? Description,
     string? Barcode,
     decimal Price,
     decimal Cost,
     Guid BrandId,
     Guid CategoryId
);