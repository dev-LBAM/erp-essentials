namespace ErpEssentials.Application.Contracts.Products.Lots;

public record ReceiveProductStockRequest(
    Guid ProductId,
    int Quantity,
    decimal PurchasePrice,
    DateTime? ExpirationDate
);