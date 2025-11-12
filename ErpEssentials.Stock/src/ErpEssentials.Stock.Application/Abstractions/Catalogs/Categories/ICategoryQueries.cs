using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;

namespace ErpEssentials.Stock.Application.Abstractions.Catalogs.Categories;

public interface ICategoryQueries
{
    Task<Result<CategoryResponse>> GetResponseByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<CategoryResponse>>> GetAllPagedAsync(int page = 1, int pageSize = 10, string orderBy = "Name", bool ascending = true, CancellationToken cancellationToken = default);
}