using ErpEssentials.Stock.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Brands.GetById;

public class GetBrandByIdQueryHandler(IBrandQueries brandQueries) : IRequestHandler<GetBrandByIdQuery, Result<BrandResponse>>
{
    private readonly IBrandQueries _brandQueries = brandQueries;
    public async Task<Result<BrandResponse>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        return await _brandQueries.GetResponseByIdAsync(request.BrandId, cancellationToken);
    }
}

