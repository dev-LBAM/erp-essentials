namespace ErpEssentials.Application.Contracts.Products;

public record ProductResponse(
    Guid Id,
    string Sku,
    string Name,
    string? Description,
    decimal Price,
    string BrandName,
    string CategoryName,
    int TotalStock
);