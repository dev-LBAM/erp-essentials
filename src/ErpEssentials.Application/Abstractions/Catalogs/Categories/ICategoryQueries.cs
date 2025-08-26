using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Application.Abstractions.Catalogs.Categories;

public interface ICategoryQueries
{
    Task<Result<CategoryResponse>> GetResponseByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
}