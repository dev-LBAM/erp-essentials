using ErpEssentials.Application.Features.Products.ReceiveStock;
using ErpEssentials.Domain.Products.Lots;
using FluentValidation.TestHelper;

namespace ErpEssentials.Application.Tests.Features.Products.ReceiveStock;

public class ReceiveProductStockCommandValidatorTests
{
    private readonly ReceiveProductStockCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        // Arrange
        var command = new ReceiveProductStockCommand(Guid.Empty, 10, 50, DateTime.UtcNow);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId)
            .WithErrorCode(LotErrors.MissingProductId.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Quantity_Is_Not_Positive()
    {
        var command = new ReceiveProductStockCommand(Guid.NewGuid(), 0, 50, DateTime.UtcNow);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void Should_Have_Error_When_PurchasePrice_Is_Negative()
    {
        var command = new ReceiveProductStockCommand(Guid.NewGuid(), 10, -5, DateTime.UtcNow);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PurchasePrice);
    }

    [Fact]
    public void Should_Not_Have_Errors_For_Valid_Command()
    {
        var command = new ReceiveProductStockCommand(Guid.NewGuid(), 10, 100, DateTime.UtcNow);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}