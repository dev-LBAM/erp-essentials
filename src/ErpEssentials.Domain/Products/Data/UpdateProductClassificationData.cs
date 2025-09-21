namespace ErpEssentials.Domain.Products.Data;

public record UpdateProductClassificationData(
    Guid? NewBrandId,
    Guid? NewCategoryId
);
