using ErpEssentials.Stock.SharedKernel.ResultPattern;

namespace ErpEssentials.Stock.Domain.Products;

public static class ProductErrors
{
    // Validation Errors
    public static readonly Error EmptySku = new DomainError("Product.EmptySku", "The product SKU cannot be empty.", ErrorType.Validation);
    public static readonly Error EmptyName = new DomainError("Product.EmptyName", "The product name cannot be empty.", ErrorType.Validation);
    public static readonly Error EmptyId = new DomainError("Product.EmptyId", "The product id cannot be empty.", ErrorType.Validation);
    public static readonly Error EmptyBrandId = new DomainError("Product.EmptyBrandId", "The product BrandId cannot be empty.", ErrorType.Validation);
    public static readonly Error EmptyCategoryId = new DomainError("Product.EmptyCategoryId", "The product CategoryId cannot be empty.", ErrorType.Validation);
    public static readonly Error EmptyFinancialUpdate = new DomainError("Product.EmptyFinancialUpdate", "At least one financial field (NewPrice or NewCost) must be provided to update the product.", ErrorType.Validation);
    public static readonly Error EmptyClassificationUpdate = new DomainError("Product.EmptyClassificationUpdate", "At least one classification field (NewBrandId or NewCategoryId) must be provided to update the product.", ErrorType.Validation);
    public static readonly Error EmptyDetailUpdate = new DomainError("Product.EmptyUpdate","At least one detail field (NewName, NewDescription or NewBarcode) must be provided to update the product.",ErrorType.Validation);


    public static readonly Error NameTooLong = new DomainError("Product.NameTooLong", "The product name cannot exceed 100 characters.", ErrorType.Validation);
    public static readonly Error DescriptionTooLong = new DomainError("Product.DescriptionTooLong", "The product description cannot exceed 500 characters.", ErrorType.Validation);
    public static readonly Error BarcodeTooLong = new DomainError("Product.BarcodeTooLong", "The product barcode cannot exceed 13 characters.", ErrorType.Validation);
    
    public static readonly Error BarcodeInUse = new DomainError("Product.BarcodeInUse", "This product barcode is already in use.", ErrorType.Conflict);
    public static readonly Error NonPositivePrice = new DomainError("Product.NonPositivePrice", "The product price must be a positive value.", ErrorType.Validation);
    public static readonly Error NonNegativeCost = new DomainError("Product.NonNegativeCost", "The product cost cannot be negative.", ErrorType.Validation);
    public static readonly Error InvalidNameFormat = new DomainError("Product.InvalidNameFormat", "The product name contains invalid characters.", ErrorType.Validation);

    public static readonly Error AlreadyInactive = new DomainError("Product.AlreadyInactive", "The product already inactive.", ErrorType.Validation);
    public static readonly Error AlreadyActive = new DomainError("Product.AlreadyActive", "The product already active.", ErrorType.Validation);

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