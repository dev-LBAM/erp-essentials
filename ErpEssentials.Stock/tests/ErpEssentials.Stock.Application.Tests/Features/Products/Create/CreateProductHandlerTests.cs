using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Application.Features.Products;
using ErpEssentials.Stock.Application.Features.Products.Create;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.SharedKernel;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.Create;

public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IProductQueries> _mockProductQueries;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockProductQueries = new Mock<IProductQueries>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new CreateProductHandler(
            _mockProductRepository.Object,
            _mockUnitOfWork.Object,
            _mockProductQueries.Object);
    }

    private static CreateProductCommand CreateValidCommand()
    {
        return new CreateProductCommand(
            Sku: "VALID-SKU",
            Name: "Test Product",
            Description: "Test Description",
            Barcode: "1234567890123",
            Price: 100,
            Cost: 50,
            BrandId: Guid.NewGuid(),
            CategoryId: Guid.NewGuid()
        );
    }
   
    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenDomainCreationSucceeds()
    {
        // Arrange
        CreateProductCommand command = CreateValidCommand();
        ProductResponse dummyResponse = new(
            Id: Guid.NewGuid(),
            Sku: command.Sku,
            Name: command.Name,
            Description: command.Description,
            Barcode: command.Barcode,
            Price: command.Price,
            Cost: command.Cost,
            BrandName: "Test Brand",
            CategoryName: "Test Category",
            CreatedAt: DateTime.UtcNow,
            UpdatedAt: null,
            TotalStock: 0,
            IsActive: true
        );
        _mockProductQueries
            .Setup(queries => queries.GetResponseByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductResponse>.Success(dummyResponse));

        // Act
        Result<ProductResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Sku, result.Value.Sku);

        _mockProductRepository.Verify(repo =>
            repo.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _mockUnitOfWork.Verify(uow =>
            uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        _mockProductQueries.Verify(queries =>
            queries.GetResponseByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenDomainEntityCreationFails()
    {
        // Arrange
        CreateProductCommand invalidCommand = CreateValidCommand() with { Price = 0 };

        // Act
        Result<ProductResponse> result = await _handler.Handle(invalidCommand, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);

        Assert.IsType<ValidationError>(result.Error);

        _mockUnitOfWork.Verify(uow =>
            uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}