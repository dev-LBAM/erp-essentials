namespace ErpEssentials.Stock.Application.Contracts.Products;

public record ReceiveProductStockRequest(
    Guid ProductId,
    int Quantity,
    decimal PurchasePrice,
    DateTime? ExpirationDate
);