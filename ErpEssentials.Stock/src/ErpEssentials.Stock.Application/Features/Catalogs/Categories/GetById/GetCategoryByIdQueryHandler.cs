using ErpEssentials.Stock.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Categories.GetById;

public class GetCategoryByIdQueryHandler(ICategoryQueries categoryQueries) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse>>
{
    private readonly ICategoryQueries _categoryQueries = categoryQueries;

    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _categoryQueries.GetResponseByIdAsync(request.CategoryId, cancellationToken);
    }
}
