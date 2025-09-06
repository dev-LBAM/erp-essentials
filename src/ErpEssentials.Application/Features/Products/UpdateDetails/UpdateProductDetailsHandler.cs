using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Data;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.UpdateDetails;

public class  UpdateProductDetailsHandler(IProductRepository productRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductDetailsCommand, Result<ProductResponse>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<ProductResponse>> Handle(UpdateProductDetailsCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)return Result<ProductResponse>.Failure(ProductErrors.NotFound);
        

        UpdateProductDetailsData updateProductDetailsData = new
        (
            request.NewBarcode,
            request.NewName,
            request.NewDescription
        );

        Result updateResult = product.UpdateDetails(updateProductDetailsData);
        if (updateResult.IsFailure)
        {
            return Result<ProductResponse>.Failure(updateResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        ProductResponse response = new(
            product.Id,
            product.Sku,
            product.Name,
            product.Description,
            product.Barcode,
            product.Price,
            product.Cost,
            product.Brand!.Name,
            product.Category!.Name,
            product.CreatedAt,
            product.UpdatedAt,
            product.GetTotalStock()
        );

        return Result<ProductResponse>.Success(response);
    }
}
