using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Catalogs.Categories;

public static class CategoryErrors
{
    public static readonly Error EmptyName = new DomainError("Category.EmptyName", "The category name cannot be empty.", ErrorType.Validation);
    public static readonly Error NameInUse = new DomainError("Category.NameInUse", "This category name is already in use.", ErrorType.Conflict);
    public static readonly Error InvalidNameFormat = new DomainError("Category.InvalidNameFormat", "The category name contains invalid characters.", ErrorType.Validation);
}