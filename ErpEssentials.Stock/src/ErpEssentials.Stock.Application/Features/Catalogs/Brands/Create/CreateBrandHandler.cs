using ErpEssentials.Stock.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using ErpEssentials.Stock.SharedKernel.Extensions;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Brands.Create;

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
        Result<Brand> brandResult = Brand.Create(request.Name);
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