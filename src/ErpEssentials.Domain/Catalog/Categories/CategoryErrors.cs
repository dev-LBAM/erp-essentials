using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Catalog.Categories;

public static class CategoryErrors
{
    public static readonly Error EmptyCategoryName = new DomainError(
        "Category.EmptyName", "The category name cannot be empty.", ErrorType.Validation);

    public static readonly Error CategoryNameInUse = new DomainError(
        "Category.NameInUse", "This category name is already in use.", ErrorType.Conflict);
}