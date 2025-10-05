using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Products.Lots;
public static class LotErrors
{
    public static readonly Error MissingProductId = new DomainError("Lot.MissingProductId", "A Lot must be associated with a ProductId.", ErrorType.Validation);
    public static readonly Error NonPositiveQuantity = new DomainError("Lot.NonPositiveQuantity", "Lot quantity must be a positive value.", ErrorType.Validation);
    public static readonly Error NonNegativePurchasePrice = new DomainError("Lot.NonNegativePurchasePrice", "Purchase price cannot be negative.", ErrorType.Validation);
    public static readonly Error InsufficientStockInLot = new DomainError("Lot.InsufficientStockInLot", "There is not enough stock in this specific lot.", ErrorType.Conflict);
    public static readonly Error NotFound = new DomainError("Lot.NotFound", "The lot with the specified ID was not found.", ErrorType.NotFound);

}
