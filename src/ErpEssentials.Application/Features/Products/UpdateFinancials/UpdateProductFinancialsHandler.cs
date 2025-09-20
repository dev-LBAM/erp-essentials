

using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Data;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.UpdateFinancials;

public class UpdateProductFinancialsHandler(
    IProductRepository productRepository, 
    IUnitOfWork unitOfWork,
    IProductQueries productQueries) : IRequestHandler<UpdateProductFinancialsCommand, Result<ProductResponse>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IProductQueries _productQueries = productQueries;
    public async Task<Result<ProductResponse>> Handle(UpdateProductFinancialsCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)return Result<ProductResponse>.Failure(ProductErrors.NotFound);
        
        UpdateProductFinancialsData updateProductFinancialsData = new
        (
            request.NewPrice,
            request.NewCost
        );

        Result<Product> updateResult = product.UpdateFinancials(updateProductFinancialsData);
        
        if (updateResult.IsFailure) return Result<ProductResponse>.Failure(updateResult.Error);
        
        Product newProduct = updateResult.Value;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _productQueries.GetResponseByIdAsync(newProduct.Id, cancellationToken);
    }
}
