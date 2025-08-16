using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Brands.Create;

public class CreateBrandHandler(IBrandRepository brandRepository) : IRequestHandler<CreateBrandCommand, Result<Brand>>
{
    private readonly IBrandRepository _brandRepository = brandRepository;


    public async Task<Result<Brand>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        string standardizedName = request.Name.ToTitleCaseStandard();

        bool isNameUnique = await _brandRepository.IsNameUniqueAsync(standardizedName, cancellationToken);
        if (!isNameUnique)
        {
            return Result<Brand>.Failure(BrandErrors.NameInUse);
        }

        Result<Brand> brandResult = Brand.Create(standardizedName);
        if (brandResult.IsFailure)
        {
            return Result<Brand>.Failure(brandResult.Error);
        }

        Brand newBrand = brandResult.Value;
        await _brandRepository.AddAsync(newBrand, cancellationToken);

        return Result<Brand>.Success(newBrand);
    }
}