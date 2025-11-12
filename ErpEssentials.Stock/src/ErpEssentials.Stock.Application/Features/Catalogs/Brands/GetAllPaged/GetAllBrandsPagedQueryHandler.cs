using ErpEssentials.Stock.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Brands.GetAllPaged;

public class GetAllBrandsPagedQueryHandler(IBrandQueries brandQueries) : IRequestHandler<GetAllBrandsPagedQuery, Result<PagedResult<BrandResponse>>>
{
    private readonly IBrandQueries _brandQueries = brandQueries;
    public async Task<Result<PagedResult<BrandResponse>>> Handle(GetAllBrandsPagedQuery request, CancellationToken cancellationToken)
    {
        return await _brandQueries.GetAllPagedAsync(request.Page, request.PageSize, request.OrderBy, request.Ascending, cancellationToken);
    }
}