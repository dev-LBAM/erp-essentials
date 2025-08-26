using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Brands.Create;

public class CreateBrandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateBrandCommand, Result<Brand>>
{
    private readonly IBrandRepository _brandRepository = brandRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;



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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Brand>.Success(newBrand);
    }
}