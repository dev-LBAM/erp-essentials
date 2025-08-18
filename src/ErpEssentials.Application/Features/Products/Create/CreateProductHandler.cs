using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Create;

public class CreateProductHandler(IProductRepository productRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, Result<Product>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product? existingProduct = await _productRepository.GetBySkuAsync(request.Sku, cancellationToken);
        
        if (existingProduct is not null) return Result<Product>.Failure(ProductErrors.SkuConflict);

        CreateProductData productData = new(
            request.Sku, 
            request.Name, 
            request.Description, 
            request.Barcode,
            request.Price, 
            request.Cost, 
            request.BrandId, 
            request.CategoryId);

        Result<Product> productResult = Product.Create(productData);

        if (productResult.IsFailure) return Result<Product>.Failure(productResult.Error);
        
        Product newProduct = productResult.Value;
        await _productRepository.AddAsync(newProduct, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Product>.Success(newProduct);
    }
}