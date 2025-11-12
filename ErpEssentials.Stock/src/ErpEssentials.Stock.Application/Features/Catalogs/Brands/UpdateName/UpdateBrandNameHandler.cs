
using ErpEssentials.Stock.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using ErpEssentials.Stock.SharedKernel.Extensions;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Brands.UpdateName;

public class UpdateBrandNameHandler(
    IBrandRepository brandRepository, 
    IUnitOfWork unitOfWork,
    IBrandQueries brandQueries) : IRequestHandler<UpdateBrandNameCommand, Result<BrandResponse>>
{
    private readonly IBrandRepository _brandRepository = brandRepository;
    private readonly IBrandQueries _brandQueries = brandQueries;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<BrandResponse>> Handle(UpdateBrandNameCommand request, CancellationToken cancellationToken)
    {
        Brand? brand = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);
        if (brand is null)
        {
            return Result<BrandResponse>.Failure(BrandErrors.NotFound);
        }

        Result<Brand> updateBrandResult = brand.UpdateName(request.NewName);
        if (updateBrandResult.IsFailure)
        {
            return Result<BrandResponse>.Failure(updateBrandResult.Error);

        }

        Brand newBrand = updateBrandResult.Value;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return await _brandQueries.GetResponseByIdAsync(newBrand.Id, cancellationToken);

    }
}