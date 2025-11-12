using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.GetById;

public class GetProductByIdQueryHandler(IProductQueries productQueries) : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    private readonly IProductQueries _productQueries= productQueries;
    

    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _productQueries.GetResponseByIdAsync(request.ProductId, cancellationToken);
    }
}