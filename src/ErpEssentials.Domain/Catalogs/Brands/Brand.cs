using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Catalogs.Brands;

public class Brand
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Brand() { }

    public static Result<Brand> Create(string name)
    {
        string standardizedName = name.ToTitleCaseStandard();

        if (string.IsNullOrWhiteSpace(standardizedName))
        {
            return Result<Brand>.Failure(BrandErrors.EmptyName);
        }

        Brand brand = new()
        {
            Id = Guid.NewGuid(),
            Name = standardizedName
        };

        return Result<Brand>.Success(brand);
    }

    public Result UpdateName(string newName)
    {
        string standardizedName = newName.ToTitleCaseStandard();

        if (string.IsNullOrWhiteSpace(standardizedName))
        {
            return Result.Failure(BrandErrors.EmptyName);
        }

        Name = standardizedName;
        return Result.Success();
    }
}