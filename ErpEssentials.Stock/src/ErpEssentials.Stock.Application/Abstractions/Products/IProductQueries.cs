using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;

namespace ErpEssentials.Stock.Application.Abstractions.Products;

public interface IProductQueries
{
    Task<Result<ProductResponse>> GetResponseByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProductResponse>>> GetAllPagedAsync(int page = 1, int pageSize = 10, string orderBy = "Name", bool ascending = true, CancellationToken cancellationToken = default);
}