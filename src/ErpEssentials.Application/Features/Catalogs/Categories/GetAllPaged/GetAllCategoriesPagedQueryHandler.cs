using ErpEssentials.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Categories.GetAllPaged;

public class GetAllCategoriesPagedQueryHandler(ICategoryQueries categoryQueries) : IRequestHandler<GetAllCategoriesPagedQuery, Result<PagedResult<CategoryResponse>>>
{
    private readonly ICategoryQueries _categoryQueries = categoryQueries;
    public async Task<Result<PagedResult<CategoryResponse>>> Handle(GetAllCategoriesPagedQuery request, CancellationToken cancellationToken)
    {
        return await _categoryQueries.GetAllPagedAsync(request.Page, request.PageSize, request.OrderBy, request.Ascending, cancellationToken);
    }
}