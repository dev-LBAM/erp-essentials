using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Catalog.Categories;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Category() { }

    public static Result<Category> Create(string name)
    {
        var standardizedName = name.ToTitleCaseStandard();

        if (string.IsNullOrWhiteSpace(standardizedName))
        {
            return Result<Category>.Failure(CategoryErrors.EmptyCategoryName);
        }

        var Category = new Category
        {
            Id = Guid.NewGuid(),
            Name = standardizedName
        };

        return Result<Category>.Success(Category);
    }

    public Result UpdateName(string newName)
    {
        var standardizedName = newName.ToTitleCaseStandard();

        if (string.IsNullOrWhiteSpace(standardizedName))
        {
            return Result.Failure(CategoryErrors.EmptyCategoryName);
        }

        Name = standardizedName;
        return Result.Success();
    }
}