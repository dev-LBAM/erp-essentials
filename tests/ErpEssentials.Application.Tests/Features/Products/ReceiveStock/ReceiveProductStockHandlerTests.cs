using ErpEssentials.Application.Abstractions.Products.Lots;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Application.Features.Products.ReceiveStock;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Data;
using ErpEssentials.Domain.Products.Lots;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Products.ReceiveStock;

public class ReceiveProductStockHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILotRepository> _mockLotRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ILotQueries> _mockLotQueries;
    private readonly ReceiveProductStockHandler _handler;

    public ReceiveProductStockHandlerTests()
    {
        _mockLotRepository = new Mock<ILotRepository>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockLotQueries = new Mock<ILotQueries>();

        _handler = new ReceiveProductStockHandler(
            _mockProductRepository.Object,
            _mockUnitOfWork.Object,
            _mockLotRepository.Object,
            _mockLotQueries.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductIdIsEmpty()
    {
        // Arrange
        var command = new ReceiveProductStockCommand(Guid.Empty, 10, 100, DateTime.UtcNow);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ProductErrors.NotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_Create_Lot_WhenDataIsValid()
    {
        // Arrange
        var brand = Brand.Create("Nike").Value;
        var category = Category.Create("Shoes").Value;
        var productData = new CreateProductData("SKU123", "Tênis", null, null, 299.99m, 100m, brand.Id, category.Id);
        var product = Product.Create(productData).Value;

        _mockProductRepository
            .Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _mockLotRepository
            .Setup(x => x.AddAsync(It.IsAny<Lot>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockLotQueries
            .Setup(x => x.GetResponseByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<LotResponse>.Success(new LotResponse(
                Guid.NewGuid(),
                product.Id,               
                50,                      
                120m,                     
                DateTime.UtcNow,          
                DateTime.UtcNow,          
                DateTime.UtcNow.AddMonths(6)
            )));


        var command = new ReceiveProductStockCommand(
            product.Id,
            50,
            120m,
            DateTime.UtcNow.AddMonths(6)
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(product.Id, result.Value.ProductId);
        Assert.Equal(50, result.Value.Quantity);

        _mockLotRepository.Verify(x => x.AddAsync(It.IsAny<Lot>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}