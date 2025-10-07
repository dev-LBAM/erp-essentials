namespace ErpEssentials.Application.Contracts.Products.Lots;

public record RemoveProductStockRequest(
    Guid ProductId,
    int Quantity
);