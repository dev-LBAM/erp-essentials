using ErpEssentials.Stock.Application.Features.Products.RemoveStock;
using ErpEssentials.Stock.Domain.Products.Lots;
using FluentValidation.TestHelper;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.RemoveStock;

public class RemoveProductStockCommandValidatorTests
{
    private readonly RemoveProductStockCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        // Arrange
        var command = new RemoveProductStockCommand(Guid.Empty, 50);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId)
            .WithErrorCode(LotErrors.MissingProductId.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Quantity_Is_Not_Positive()
    {
        var command = new RemoveProductStockCommand(Guid.NewGuid(), -50);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }


    [Fact]
    public void Should_Not_Have_Errors_For_Valid_Command()
    {
        var command = new RemoveProductStockCommand(Guid.NewGuid(), 100);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}