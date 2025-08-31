
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Brands.UpdateName;

public class UpdateBrandNameHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateBrandNameCommand, Result>
{
    private readonly IBrandRepository _brandRepository = brandRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateBrandNameCommand request, CancellationToken cancellationToken)
    {
        Brand? brand = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);
        if (brand is null)
        {
            return Result.Failure(BrandErrors.NotFound);
        }

        string standardizedName = request.NewName.ToTitleCaseStandard();

        bool isNameUnique = await _brandRepository.IsNameUniqueAsync(standardizedName, cancellationToken);
        if (!isNameUnique)
        {
            return Result.Failure(BrandErrors.NameInUse);
        }

        Result updateResult = brand.UpdateName(request.NewName);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
