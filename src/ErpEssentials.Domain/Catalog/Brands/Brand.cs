using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Catalog.Brands;

public class Brand
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Brand() { }

    public static Result<Brand> Create(string name)
    {
        var standardizedName = name.ToTitleCaseStandard();

        if (string.IsNullOrWhiteSpace(standardizedName))
        {
            return Result<Brand>.Failure(BrandErrors.EmptyBrandName);
        }

        var brand = new Brand
        {
            Id = Guid.NewGuid(),
            Name = standardizedName
        };

        return Result<Brand>.Success(brand);
    }

    public Result UpdateName(string newName)
    {
        var standardizedName = newName.ToTitleCaseStandard();

        if (string.IsNullOrWhiteSpace(standardizedName))
        {
            return Result.Failure(BrandErrors.EmptyBrandName);
        }

        Name = standardizedName;
        return Result.Success();
    }
}