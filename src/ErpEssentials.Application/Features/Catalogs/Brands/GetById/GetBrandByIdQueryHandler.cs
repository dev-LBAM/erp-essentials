using ErpEssentials.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Brands.GetById;

public class GetBrandByIdQueryHandler(IBrandQueries brandQueries) : IRequestHandler<GetBrandByIdQuery, Result<BrandResponse>>
{
    private readonly IBrandQueries _brandQueries = brandQueries;
    public async Task<Result<BrandResponse>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        return await _brandQueries.GetResponseByIdAsync(request.BrandId, cancellationToken);
    }
}

