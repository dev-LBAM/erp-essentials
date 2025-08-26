using ErpEssentials.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Categories.GetById;

public class GetCategoryByIdQueryHandler(ICategoryQueries categoryQueries) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse>>
{
    private readonly ICategoryQueries _categoryQueries = categoryQueries;

    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _categoryQueries.GetResponseByIdAsync(request.CategoryId, cancellationToken);
    }
}
