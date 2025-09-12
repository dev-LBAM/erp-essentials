using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Data;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Create;

public class CreateProductHandler
    (IProductRepository productRepository, IUnitOfWork unitOfWork, IProductQueries productQueries) 
    : IRequestHandler<CreateProductCommand, Result<ProductResponse>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IProductQueries _productQueries = productQueries;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        CreateProductData createProductData = new(
            request.Sku, 
            request.Name, 
            request.Description, 
            request.Barcode,
            request.Price, 
            request.Cost, 
            request.BrandId, 
            request.CategoryId);

        Result<Product> productResult = Product.Create(createProductData);

        if (productResult.IsFailure) return Result<ProductResponse>.Failure(productResult.Error);
        
        Product newProduct = productResult.Value;
        await _productRepository.AddAsync(newProduct, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _productQueries.GetResponseByIdAsync(newProduct.Id, cancellationToken);
    }
}