using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Data;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.UpdateClassification;

public class UpdateProductClassificationHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IProductQueries productQueries) : IRequestHandler<UpdateProductClassificationCommand, Result<ProductResponse>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IProductQueries _productQueries = productQueries;
    public async Task<Result<ProductResponse>> Handle(UpdateProductClassificationCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<ProductResponse>.Failure(ProductErrors.NotFound);

        UpdateProductClassificationData updateProductClassificationData = new
        (
            request.NewBrandId,
            request.NewCategoryId
        );

        Result<Product> updateResult = product.UpdateClassification(updateProductClassificationData);

        if (updateResult.IsFailure) return Result<ProductResponse>.Failure(updateResult.Error);

        Product newProduct = updateResult.Value;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _productQueries.GetResponseByIdAsync(newProduct.Id, cancellationToken);
    }
}
