namespace ErpEssentials.Application.Contracts.Products;

public record RemoveProductStockRequest(
    Guid ProductId,
    int Quantity
);