
using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Application.Features.Products.UpdateFinancials;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.Domain.Products.Data;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.UpdateFinancials;

public class UpdateProductFinancialsHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductQueries> _mockProductQueries;
    private readonly UpdateProductFinancialsHandler _handler;

    private static Product CreateTestProduct()
    {
        Brand brand = Brand.Create("Test Brand").Value;
        Category category = Category.Create("Test Category").Value;
        CreateProductData initialData = new("SKU", "Old Name", "Old Desc", "Old Barcode", 100, 50, brand.Id, category.Id);
        Product product = Product.Create(initialData).Value;

        typeof(Product).GetProperty(nameof(Product.Brand))?.SetValue(product, brand, null);
        typeof(Product).GetProperty(nameof(Product.Category))?.SetValue(product, category, null);
        return product;
    }

    public UpdateProductFinancialsHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductQueries = new Mock<IProductQueries>();
        _handler = new UpdateProductFinancialsHandler(
            _mockProductRepository.Object,
            _mockUnitOfWork.Object,
            _mockProductQueries.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductIsNotFound()
    {
        // Arrange
        UpdateProductFinancialsCommand command = new(Guid.NewGuid(), 15.5m, 5.0m);
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
    public async Task Handle_Should_ReturnFailure_WhenDomainUpdateFails()
    {
        // Arrange
        Product product = CreateTestProduct();
        UpdateProductFinancialsCommand command = new(product.Id, 0m, -1);

        _mockProductRepository
            .Setup(repo => repo.GetByIdAsync(command.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        Result<ProductResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_And_MapToResponse_WhenUpdateIsValid()
    {
        // Arrange
        Product product = CreateTestProduct();
        UpdateProductFinancialsCommand command = new(ProductId: product.Id, NewPrice: 15m, NewCost: 5m);
        decimal? price = command.NewPrice;
        decimal? cost = command.NewCost;
        ProductResponse dummyResponse = new(
            Id: Guid.NewGuid(),
            Sku: product.Sku,
            Name: product.Name,
            Description: product.Description,
            Barcode: product.Barcode,
            Price: price ?? 0m,
            Cost: cost ?? 0m,
            BrandName: "Test Brand",
            CategoryName: "Test Category",
            CreatedAt: DateTime.UtcNow,
            UpdatedAt: null,
            TotalStock: 0,
            IsActive: product.IsActive
        );
        _mockProductRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _mockProductQueries
            .Setup(queries => queries.GetResponseByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductResponse>.Success(dummyResponse));

        // Act
        Result<ProductResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        ProductResponse response = result.Value;
        Assert.Equal(dummyResponse.Price, response.Price);
        Assert.Equal(dummyResponse.Cost, response.Cost);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
