namespace ErpEssentials.Stock.Domain.Products.Data;

public record UpdateProductDetailsData(
    string? NewBarcode,
    string? NewName,
    string? NewDescription
);
