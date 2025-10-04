

using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Features.Products.Deactivate;
using ErpEssentials.Application.Features.Products.UpdateClassification;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Data;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Products.Deactivate;

public class DeactivateProductHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductQueries> _mockProductQueries;
    private readonly DeactivateProductHandler _handler;

    private static Product CreateTestProduct()
    {
        Brand brand = Brand.Create("Test Brand").Value;
        Category category = Category.Create("Test Category").Value;
        CreateProductData initialData = new(
            Sku: "SKU",
            Name: "Old Name",
            Description: "Old Desc",
            Barcode: "Old Barcode",
            Price: 100m,
            Cost: 50m,
            BrandId: brand.Id,
            CategoryId: category.Id
        );
        Product product = Product.Create(initialData).Value;

        typeof(Product).GetProperty(nameof(Product.Brand))?.SetValue(product, brand, null);
        typeof(Product).GetProperty(nameof(Product.Category))?.SetValue(product, category, null);
        return product;
    }
    public DeactivateProductHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductQueries = new Mock<IProductQueries>();
        _handler = new DeactivateProductHandler(
            _mockProductRepository.Object,
            _mockUnitOfWork.Object,
            _mockProductQueries.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductIsNotFound()
    {
        // Arrange
        DeactivateProductCommand command = new(Guid.NewGuid());

        _mockProductRepository
            .Setup(repo => repo.GetByIdAsync(command.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        Result<ProductResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ProductErrors.NotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductIsAlreadyInactive()
    {
        // Arrange
        Product product = CreateTestProduct();
        typeof(Product).GetProperty(nameof(Product.IsActive))?.SetValue(product, false, null);
        DeactivateProductCommand command = new(product.Id);
        _mockProductRepository
            .Setup(repo => repo.GetByIdAsync(command.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        // Act
        Result<ProductResponse> result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ProductErrors.AlreadyInactive.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_DeactivateProductSuccessfully()
    {
        // Arrange
        Product product = CreateTestProduct();
        DeactivateProductCommand command = new(product.Id);
        _mockProductRepository
            .Setup(repo => repo.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _mockProductQueries
            .Setup(q => q.GetResponseByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductResponse>.Success(new ProductResponse(
                product.Id,
                product.Sku,
                product.Name,
                product.Description,
                product.Barcode,
                product.Price,
                product.Cost,
                BrandName: "New Brand",
                CategoryName: "New Category",
                CreatedAt: product.CreatedAt,
                UpdatedAt: DateTime.UtcNow,
                TotalStock: 0,
                IsActive: false
            )));

        // Act
        Result<ProductResponse> result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}


