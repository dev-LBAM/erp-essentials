using ErpEssentials.Stock.Application.Features.Products.Lots.AddQuantityToLot;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.Domain.Products.Data;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using Moq;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.Lots.AddQuantityToLot;

public class AddQuantityToLotHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly AddQuantityToLotHandler _handler;

    public AddQuantityToLotHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new AddQuantityToLotHandler(
            _mockProductRepository.Object,
            _mockUnitOfWork.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductNotFound()
    {
        // Arrange
        var command = new AddQuantityToLotCommand(Guid.Empty, Guid.NewGuid(), 100);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ProductErrors.NotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_Add_Quantity_To_Lot_WhenDataIsValid()
    {
        // Arrange
        var brand = Brand.Create("Nike").Value;
        var category = Category.Create("Shoes").Value;
        var productData = new CreateProductData("SKU123", "Tênis", null, null, 299.99m, 100m, brand.Id, category.Id);
        var product = Product.Create(productData).Value;
        var lotData = new CreateLotData(100, 299.99m, DateTime.UtcNow.AddMonths(1));
        var lot = product.ReceiveStock(lotData).Value;

        _mockProductRepository
            .Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);


        var command = new AddQuantityToLotCommand
        (
            ProductId: product.Id,
            LotId: lot.Id,
            Quantity: 10
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}