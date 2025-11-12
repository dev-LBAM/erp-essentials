namespace ErpEssentials.Stock.Application.Contracts.Products;

public record UpdateProductFinancialsRequest(
    decimal? NewPrice,
    decimal? NewCost
);