namespace ErpEssentials.Application.Contracts.Products;

public record ProductResponse(
    Guid Id,
    string Sku,
    string Name,
    string? Description,
    string? Barcode,
    decimal Price,
    decimal Cost,
    string BrandName,
    string CategoryName,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int TotalStock,
    bool IsActive
);