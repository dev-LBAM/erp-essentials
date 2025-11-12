using ErpEssentials.Stock.SharedKernel.ResultPattern;

namespace ErpEssentials.Stock.Domain.Catalogs.Categories;

public static class CategoryErrors
{
    public static readonly Error EmptyName = new DomainError("Category.EmptyName", "The category name cannot be empty.", ErrorType.Validation);
    public static readonly Error EmptyId = new DomainError("Category.EmptyId", "The category id cannot be empty.", ErrorType.Validation);
    public static readonly Error NameInUse = new DomainError("Category.NameInUse", "This category name is already in use.", ErrorType.Conflict);
    public static readonly Error InvalidNameFormat = new DomainError("Category.InvalidNameFormat", "The category name contains invalid characters.", ErrorType.Validation);
    public static readonly Error NotFound = new DomainError("Category.NotFound", "The category with the specified ID was not found.", ErrorType.NotFound);
    public static readonly Error NameTooLong = new DomainError("Category.NameTooLong", "The category name cannot exceed 150 characters.", ErrorType.Validation);
}