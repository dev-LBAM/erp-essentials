using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Features.Products.UpdateDetails;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Data;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Products.UpdateDetails;

public class UpdateProductDetailsHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductQueries> _mockProductQueries;
    private readonly UpdateProductDetailsHandler _handler;

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

    public UpdateProductDetailsHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductQueries = new Mock<IProductQueries>();
        _handler = new UpdateProductDetailsHandler(
            _mockProductRepository.Object, 
            _mockUnitOfWork.Object,
            _mockProductQueries.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductIsNotFound()
    {
        // Arrange
        UpdateProductDetailsCommand command = new(Guid.NewGuid(), "New Name", null, null);
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
        UpdateProductDetailsCommand command = new(product.Id, "", null, null);

        _mockProductRepository
            .Setup(repo => repo.GetByIdAsync(command.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        Result<ProductResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ProductErrors.EmptyName.Code, result.Error.Code);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_And_MapToResponse_WhenUpdateIsValid()
    {
        // Arrange
        Product product = CreateTestProduct();
        UpdateProductDetailsCommand command = new(product.Id, "New Name", "New Desc", "New Barcode");
        ProductResponse dummyResponse = new(
            Id: Guid.NewGuid(),
            Sku: product.Sku,
            Name: command.NewName!,
            Description: command.NewDescription,
            Barcode: command.NewBarcode,
            Price: product.Price,
            Cost: product.Cost,
            BrandName: "Test Brand",
            CategoryName: "Test Category",
            CreatedAt: DateTime.UtcNow,
            UpdatedAt: null,
            TotalStock: 0
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
        Assert.Equal(dummyResponse.Name, response.Name);
        Assert.Equal(dummyResponse.Description, response.Description);
        Assert.Equal(dummyResponse.Barcode, response.Barcode);
        Assert.Equal(dummyResponse.BrandName, response.BrandName);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}