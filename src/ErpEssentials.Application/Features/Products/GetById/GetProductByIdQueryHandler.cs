using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.GetById;

public class GetProductByIdQueryHandler(IProductQueries productQueries) : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    private readonly IProductQueries _productQueries= productQueries;
    

    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _productQueries.GetResponseByIdAsync(request.ProductId, cancellationToken);
    }
}