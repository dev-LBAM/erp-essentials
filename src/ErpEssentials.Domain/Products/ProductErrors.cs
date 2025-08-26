using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Products;

public static class ProductErrors
{
    // Validation Errors
    public static readonly Error EmptySku = new DomainError("Product.EmptySku", "SKU cannot be empty.", ErrorType.Validation);
    public static readonly Error EmptyName = new DomainError("Product.EmptyName", "Name cannot be empty.", ErrorType.Validation);
    public static readonly Error NonPositivePrice = new DomainError("Product.NonPositivePrice", "Price must be a positive value.", ErrorType.Validation);
    public static readonly Error NonNegativeCost = new DomainError("Product.NonNegativeCost", "Cost cannot be negative.", ErrorType.Validation);
    public static readonly Error EmptyBrandId = new DomainError("Product.EmptyBrandId", "BrandId cannot be empty.", ErrorType.Validation);
    public static readonly Error EmptyCategoryId = new DomainError("Product.EmptyCategoryId", "CategoryId cannot be empty.", ErrorType.Validation);
    public static readonly Error InvalidNameFormat = new DomainError("Product.InvalidNameFormat", "The product name contains invalid characters.", ErrorType.Validation);

    // Business Rule Errors
    public static readonly Error InvalidStockQuantity = new DomainError("Product.InvalidStockQuantity", "Stock quantity must be positive.", ErrorType.Validation);
    public static readonly Error InsufficientStock = new DomainError("Product.InsufficientStock", "There is not enough stock to fulfill the request.", ErrorType.Conflict);
    public static readonly Error LotNotFound = new DomainError("Product.LotNotFound", "The specified lot was not found for this product.", ErrorType.NotFound);
    public static readonly Error InvalidLotQuantity = new DomainError("Product.InvalidLotQuantity", "Lot quantity must be a positive value.", ErrorType.Validation);
    public static readonly Error InsufficientStockInLot = new DomainError("Product.InsufficientStockInLot", "There is not enough stock in this specific lot.", ErrorType.Conflict);

    // Query Errors
    public static readonly Error NotFound = new DomainError("Product.NotFound", "The product with the specified ID was not found.", ErrorType.NotFound);
    public static readonly Error SkuConflict = new DomainError("Product.SkuConflict", "A product with this SKU already exists.", ErrorType.Conflict);
}