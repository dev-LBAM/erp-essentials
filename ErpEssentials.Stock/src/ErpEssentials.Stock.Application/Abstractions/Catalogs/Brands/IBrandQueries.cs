using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;

namespace ErpEssentials.Stock.Application.Abstractions.Catalogs.Brands;

public interface IBrandQueries
{
    Task<Result<BrandResponse>> GetResponseByIdAsync(Guid brandId, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BrandResponse>>> GetAllPagedAsync(int page = 1, int pageSize = 10, string orderBy = "Name", bool ascending = true, CancellationToken cancellationToken = default);
}