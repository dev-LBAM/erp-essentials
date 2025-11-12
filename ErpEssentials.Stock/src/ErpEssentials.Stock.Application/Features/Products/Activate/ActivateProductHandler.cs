using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Activate;

public class ActivateProductHandler(
    IProductRepository productRepository, 
    IUnitOfWork unitOfWork, 
    IProductQueries productQueries)  : IRequestHandler<ActivateProductCommand, Result<ProductResponse>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IProductQueries _productQueries = productQueries;
    
    public async Task<Result<ProductResponse>> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<ProductResponse>.Failure(ProductErrors.NotFound);

        Result<Product> activateResult = product.Activate();

        if (activateResult.IsFailure) return Result<ProductResponse>.Failure(activateResult.Error);
        
        Product activatedProduct = activateResult.Value;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _productQueries.GetResponseByIdAsync(activatedProduct.Id, cancellationToken);
    }
}