namespace ErpEssentials.Application.Contracts.Products;

public record UpdateProductFinancialsRequest(
    decimal? NewPrice,
    decimal? NewCost
);