namespace ErpEssentials.Application.Contracts.Products.Lots;

public record LotResponse(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal PurchasePrice,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? ExpirationDate
);