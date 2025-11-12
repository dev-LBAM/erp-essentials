using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.Domain.Products.Data;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.UpdateClassification;

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
