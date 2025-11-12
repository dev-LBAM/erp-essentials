namespace ErpEssentials.Stock.Domain.Products.Data;

public record UpdateProductFinancialsData(
    decimal? NewPrice,
    decimal? NewCost
);
