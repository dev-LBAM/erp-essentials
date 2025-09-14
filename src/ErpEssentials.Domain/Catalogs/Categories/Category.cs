using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Catalogs.Categories;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Category() { }

    public static Result<Category> Create(string name)
    {
        string standardizedName = name.ToTitleCaseStandard();

        if (string.IsNullOrWhiteSpace(standardizedName))
        {
            return Result<Category>.Failure(CategoryErrors.EmptyName);
        }

        Category Category = new()
        {
            Id = Guid.NewGuid(),
            Name = standardizedName
        };

        return Result<Category>.Success(Category);
    }

    public Result<Category> UpdateName(string newName)
    {
        string standardizedName = newName.ToTitleCaseStandard();

        if (string.IsNullOrWhiteSpace(standardizedName))
        {
            return Result<Category>.Failure(CategoryErrors.EmptyName);
        }

        Name = standardizedName;
        return Result<Category>.Success(this);
    }
}