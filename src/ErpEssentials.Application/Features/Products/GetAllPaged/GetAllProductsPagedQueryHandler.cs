using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.GetAllPaged;

public class GetAllProductsPagedQueryHandler(IProductQueries productQueries) : IRequestHandler<GetAllProductsPagedQuery, Result<PagedResult<ProductResponse>>>
{
    private readonly IProductQueries _productQueries = productQueries;
    public async Task<Result<PagedResult<ProductResponse>>> Handle(GetAllProductsPagedQuery request, CancellationToken cancellationToken)
    {
        return await _productQueries.GetAllPagedAsync(request.Page, request.PageSize, request.OrderBy, request.Ascending, cancellationToken);
    }
}