using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using ErpEssentials.Domain.Products.Lots;
using ErpEssentials.Domain.Products.Data;

namespace ErpEssentials.Domain.Tests.Products;

public class ProductTests
{
    private static CreateProductData CreateValidProductData()
    {
        return new CreateProductData(
            Sku: "VALID-SKU-001",
            Name: "Valid Product Name",
            Description: "Valid description for the product",
            Barcode: "1234567890123",
            Price: 100m,
            Cost: 50m,
            BrandId: Guid.NewGuid(),
            CategoryId: Guid.NewGuid()
        );
    }

    public class Create_Method
    {
        [Fact]
        public void Should_Succeed_WhenAllDataIsValid()
        {
            // Arrange
            CreateProductData validData = CreateValidProductData();

            // Act
            Result<Product> result = Product.Create(validData);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(validData.Name, result.Value.Name);
            Assert.Equal(validData.Sku, result.Value.Sku);
            Assert.Equal(validData.Description, result.Value.Description);
            Assert.Equal(validData.Barcode, result.Value.Barcode);
            Assert.Equal(validData.Price, result.Value.Price);
            Assert.Equal(validData.Cost, result.Value.Cost);
        }

        [Fact]
        public void Should_Fail_And_AggregateAllErrors_WhenMultipleRulesAreBroken()
        {
            // Arrange
            CreateProductData invalidData = new(
                Sku: "",
                Name: " ",
                Description: null,
                Barcode: null,
                Price: -10m,
                Cost: -50m,
                BrandId: Guid.NewGuid(),
                CategoryId: Guid.Empty
            );

            // Act
            Result<Product> result = Product.Create(invalidData);

            // Assert
            Assert.True(result.IsFailure);
            ValidationError validationError = Assert.IsType<ValidationError>(result.Error);
            Assert.Equal(5, validationError.Errors.Count);
            Assert.True(validationError.Errors.ContainsKey("EmptySku"));
            Assert.True(validationError.Errors.ContainsKey("EmptyName"));
            Assert.True(validationError.Errors.ContainsKey("NonPositivePrice"));
            Assert.True(validationError.Errors.ContainsKey("NonNegativeCost"));
            Assert.True(validationError.Errors.ContainsKey("EmptyCategoryId"));
        }
    }

    public class Stock_Management_Methods
    {
        private readonly Product _product;

        public Stock_Management_Methods()
        {
            _product = Product.Create(CreateValidProductData()).Value;
        }

        [Fact]
        public void ReceiveStock_Should_AddLotToProduct_WhenDataIsValid()
        {
            // Arrange
            CreateLotData newLotData = new(10, 50m, DateTime.UtcNow.AddDays(30));

            // Act
            Result result = _product.ReceiveStock(newLotData);

            Lot? addedLot = _product.Lots[0];

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(_product.Lots);
            Assert.Equal(10, _product.GetTotalStock());
            Assert.NotNull(addedLot);
            Assert.Equal(10, addedLot.Quantity);
        }

        [Fact]
        public void RemoveStock_Should_Succeed_And_ConsumeLotsInFEFO_Order()
        {
            // Arrange
            _product.ReceiveStock(new CreateLotData(10, 52m, DateTime.UtcNow.AddDays(5)));
            _product.ReceiveStock(new CreateLotData(10, 50m, DateTime.UtcNow.AddDays(10)));
            _product.ReceiveStock(new CreateLotData(10, 48m, null));   

            // Act
            Result result = _product.RemoveStock(15);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, _product.Lots.Count);
            Assert.Equal(15, _product.GetTotalStock());

            Lot? partiallyConsumedLot = _product.Lots.FirstOrDefault(l => l.ExpirationDate.HasValue);
            Assert.NotNull(partiallyConsumedLot);
            Assert.Equal(5, partiallyConsumedLot.Quantity);
        }

        [Fact]
        public void RemoveStock_Should_Fail_WhenTotalStockIsInsufficient()
        {
            // Arrange
            _product.ReceiveStock(new CreateLotData(10, 50m, null));

            // Act
            Result result = _product.RemoveStock(15);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ProductErrors.InsufficientStock.Code, result.Error.Code);
        }

        [Fact]
        public void RemoveQuantityFromLot_Should_Fail_WhenLotIsNotFound()
        {
            // Arrange
            Guid nonExistentLotId = Guid.NewGuid();

            // Act
            Result result = _product.RemoveQuantityFromLot(nonExistentLotId, 5);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ProductErrors.LotNotFound.Code, result.Error.Code);
        }

        [Fact]
        public void AddQuantityToLot_Should_Succeed_WhenLotExists()
        {
            // Arrange
            _product.ReceiveStock(new CreateLotData(10, 50m, null));
            Guid lotId = _product.Lots[0].Id;
            int quantityToAdd = 5;

            // Act
            Result result = _product.AddQuantityToLot(lotId, quantityToAdd);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(15, _product.GetTotalStock());
            Assert.Equal(15, _product.Lots[0].Quantity);
        }

        [Fact]
        public void AddQuantityToLot_Should_Fail_WhenLotDoesNotExist()
        {
            // Arrange
            Guid nonExistentLotId = Guid.NewGuid();
            int quantityToAdd = 5;

            // Act
            Result result = _product.AddQuantityToLot(nonExistentLotId, quantityToAdd);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ProductErrors.LotNotFound.Code, result.Error.Code);
        }

        [Fact]
        public void RemoveQuantityFromLot_Should_Succeed_WhenLotHasSufficientStock()
        {
            // Arrange
            _product.ReceiveStock(new CreateLotData(20, 50m, null));
            Guid lotId = _product.Lots[0].Id;
            int quantityToRemove = 5;

            // Act
            Result result = _product.RemoveQuantityFromLot(lotId, quantityToRemove);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(15, _product.GetTotalStock());
            Assert.Equal(15, _product.Lots[0].Quantity);
        }

        [Fact]
        public void RemoveQuantityFromLot_Should_Fail_WhenStockInLotIsInsufficient()
        {
            // Arrange
            _product.ReceiveStock(new CreateLotData(10, 50m, null));
            Guid lotId = _product.Lots[0].Id;
            int quantityToRemove = 15;

            // Act
            Result result = _product.RemoveQuantityFromLot(lotId, quantityToRemove);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ProductErrors.InsufficientStockInLot.Code, result.Error.Code);
            Assert.Equal(10, _product.GetTotalStock());
            Assert.Equal(10, _product.Lots[0].Quantity);
        }

        [Fact]
        public void RemoveQuantityFromLot_Should_RemoveLot_WhenQuantityReachesZero()
        {
            // Arrange
            _product.ReceiveStock(new CreateLotData(10, 50m, null));
            Guid lotId = _product.Lots[0].Id;
            int quantityToRemove = 10;

            // Act
            Result result = _product.RemoveQuantityFromLot(lotId, quantityToRemove);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(_product.Lots);
            Assert.Equal(0, _product.GetTotalStock());
        }
    }
}