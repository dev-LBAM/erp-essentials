using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.GetAllPaged;

public class GetAllProductsPagedQueryHandler(IProductQueries productQueries) : IRequestHandler<GetAllProductsPagedQuery, Result<PagedResult<ProductResponse>>>
{
    private readonly IProductQueries _productQueries = productQueries;
    public async Task<Result<PagedResult<ProductResponse>>> Handle(GetAllProductsPagedQuery request, CancellationToken cancellationToken)
    {
        return await _productQueries.GetAllPagedAsync(request.Page, request.PageSize, request.OrderBy, request.Ascending, cancellationToken);
    }
}