using ErpEssentials.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Brands.GetAllPaged;

public class GetAllBrandsPagedQueryHandler(IBrandQueries brandQueries) : IRequestHandler<GetAllBrandsPagedQuery, Result<PagedResult<BrandResponse>>>
{
    private readonly IBrandQueries _brandQueries = brandQueries;
    public async Task<Result<PagedResult<BrandResponse>>> Handle(GetAllBrandsPagedQuery request, CancellationToken cancellationToken)
    {
        return await _brandQueries.GetAllPagedAsync(request.Page, request.PageSize, request.OrderBy, request.Ascending, cancellationToken);
    }
}