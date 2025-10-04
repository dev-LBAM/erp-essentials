using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Deactivate;

public class DeactivateProductHandler(
    IProductRepository productRepository, 
    IUnitOfWork unitOfWork, 
    IProductQueries productQueries) : IRequestHandler<DeactivateProductCommand, Result<ProductResponse>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IProductQueries _productQueries = productQueries;

    public async Task<Result<ProductResponse>> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<ProductResponse>.Failure(ProductErrors.NotFound);

        Result<Product> deactivateResult = product.Deactivate();

        if (deactivateResult.IsFailure) return Result<ProductResponse>.Failure(deactivateResult.Error);
        
        Product deactivatedProduct = deactivateResult.Value;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _productQueries.GetResponseByIdAsync(deactivatedProduct.Id, cancellationToken);
    }
}