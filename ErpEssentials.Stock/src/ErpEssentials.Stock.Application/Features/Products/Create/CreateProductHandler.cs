using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.Domain.Products.Data;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Create;

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