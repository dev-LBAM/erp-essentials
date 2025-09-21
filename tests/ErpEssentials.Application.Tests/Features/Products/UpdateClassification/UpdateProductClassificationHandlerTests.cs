using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Features.Products.UpdateClassification;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Data;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Products.UpdateClassification;

public class UpdateProductClassificationHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductQueries> _mockProductQueries;
    private readonly UpdateProductClassificationHandler _handler;

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

    public UpdateProductClassificationHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductQueries = new Mock<IProductQueries>();
        _handler = new UpdateProductClassificationHandler(
            _mockProductRepository.Object,
            _mockUnitOfWork.Object,
            _mockProductQueries.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductIsNotFound()
    {
        // Arrange
        UpdateProductClassificationCommand command = new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
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

        UpdateProductClassificationCommand command = new(product.Id, Guid.Empty, Guid.Empty);

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
    public async Task Handle_Should_UpdateBrandAndCategory_WhenCommandIsValid()
    {
        // Arrange
        Product product = CreateTestProduct();
        Guid newBrandId = Guid.NewGuid();
        Guid newCategoryId = Guid.NewGuid();

        UpdateProductClassificationCommand command = new(
            product.Id,
            NewBrandId: newBrandId,
            NewCategoryId: newCategoryId
        );

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
                TotalStock: 0
            )));

        // Act
        Result<ProductResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(newBrandId, product.BrandId);
        Assert.Equal(newCategoryId, product.CategoryId);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
