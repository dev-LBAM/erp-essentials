namespace ErpEssentials.Application.Contracts.Products;

public record UpdateProductClassificationRequest(
    Guid? NewBrandId,
    Guid? NewCategoryId
);