using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Application.Abstractions.Catalogs.Brands;

public interface IBrandQueries
{
    Task<Result<BrandResponse>> GetResponseByIdAsync(Guid brandId, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BrandResponse>>> GetAllPagedAsync(int page = 1, int pageSize = 10, string orderBy = "Name", bool ascending = true, CancellationToken cancellationToken = default);
}