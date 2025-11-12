using ErpEssentials.Stock.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Categories.GetAllPaged;

public class GetAllCategoriesPagedQueryHandler(ICategoryQueries categoryQueries) : IRequestHandler<GetAllCategoriesPagedQuery, Result<PagedResult<CategoryResponse>>>
{
    private readonly ICategoryQueries _categoryQueries = categoryQueries;
    public async Task<Result<PagedResult<CategoryResponse>>> Handle(GetAllCategoriesPagedQuery request, CancellationToken cancellationToken)
    {
        return await _categoryQueries.GetAllPagedAsync(request.Page, request.PageSize, request.OrderBy, request.Ascending, cancellationToken);
    }
}