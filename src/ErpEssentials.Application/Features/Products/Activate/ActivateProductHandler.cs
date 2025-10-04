using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Activate;

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