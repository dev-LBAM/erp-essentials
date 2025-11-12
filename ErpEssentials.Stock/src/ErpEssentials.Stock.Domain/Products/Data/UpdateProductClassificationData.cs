namespace ErpEssentials.Stock.Domain.Products.Data;

public record UpdateProductClassificationData(
    Guid? NewBrandId,
    Guid? NewCategoryId
);
