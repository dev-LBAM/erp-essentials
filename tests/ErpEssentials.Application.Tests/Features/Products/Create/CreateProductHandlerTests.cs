using ErpEssentials.Application.Features.Products.Create;
using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using ErpEssentials.SharedKernel.Abstractions;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Products.Create;

public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateProductHandler(_mockProductRepository.Object,
            _mockUnitOfWork.Object);
    }

    private static CreateProductCommand CreateValidCommand(string sku = "UNIQUE-SKU")
    {
        return new CreateProductCommand
        {
            Sku = sku,
            Name = "Test Product",
            Description = "Test Description",
            Price = 100,
            Cost = 50,
            BrandId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid()
        };
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenSkuIsNotUnique()
    {
        // Arrange
        CreateProductCommand command = CreateValidCommand("DUPLICATE-SKU");

        Product existingProduct = Product.Create(
            new CreateProductData(
                command.Sku, "Existing Name", null, null, 1, 1, Guid.NewGuid(), Guid.NewGuid())
        ).Value;

        _mockProductRepository
            .Setup(repo => repo.GetBySkuAsync(command.Sku, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        // Act
        Result<Product> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Product.SkuConflict", result.Error.Code);
        _mockProductRepository.Verify(repo =>
            repo.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _mockUnitOfWork.Verify(uow =>
            uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_CallAddAsyncAndReturnSuccess_WhenSkuIsUnique()
    {
        // Arrange
        CreateProductCommand command = CreateValidCommand();

        _mockProductRepository
            .Setup(repo => repo.GetBySkuAsync(command.Sku, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        Result<Product> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        _mockProductRepository.Verify(repo =>
            repo.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _mockUnitOfWork.Verify(uow =>
            uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}