using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Application.Abstractions.Catalogs.Categories;

public interface ICategoryQueries
{
    Task<Result<CategoryResponse>> GetResponseByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<CategoryResponse>>> GetAllPagedAsync(int page = 1, int pageSize = 10, string orderBy = "Name", bool ascending = true, CancellationToken cancellationToken = default);
}