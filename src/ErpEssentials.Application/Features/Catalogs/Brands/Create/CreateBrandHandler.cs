using ErpEssentials.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Brands.Create;

public class CreateBrandHandler(
    IBrandRepository brandRepository, 
    IUnitOfWork unitOfWork,
    IBrandQueries brandQueries) : IRequestHandler<CreateBrandCommand, Result<BrandResponse>>
{
    private readonly IBrandRepository _brandRepository = brandRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBrandQueries _brandQueries = brandQueries;

    public async Task<Result<BrandResponse>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        string standardizedName = request.Name.ToTitleCaseStandard();

        bool isNameUnique = await _brandRepository.IsNameUniqueAsync(standardizedName, cancellationToken);
        if (!isNameUnique)
        {
            return Result<BrandResponse>.Failure(BrandErrors.NameInUse);
        }

        Result<Brand> brandResult = Brand.Create(standardizedName);
        if (brandResult.IsFailure)
        {
            return Result<BrandResponse>.Failure(brandResult.Error);
        }

        Brand newBrand = brandResult.Value;
        await _brandRepository.AddAsync(newBrand, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _brandQueries.GetResponseByIdAsync(newBrand.Id, cancellationToken);

    }
}