using ErpEssentials.Stock.Application.Features.Products.Lots.AddQuantityToLot;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.Domain.Products.Lots;
using FluentValidation.TestHelper;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.Lots.AddQuantityToLot;

public class AddQuantityToLotCommandValidatorTests
{
    private readonly AddQuantityToLotCommandValidator _validator;

    public AddQuantityToLotCommandValidatorTests()
    {
        _validator = new AddQuantityToLotCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        // Arrange
        var command = new AddQuantityToLotCommand
        (
            ProductId: Guid.Empty,
            LotId: Guid.NewGuid(),
            Quantity: 10
        );
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldHaveValidationErrorFor(p => p.ProductId)
            .WithErrorMessage(ProductErrors.NotFound.Message);
    }

    [Fact]
    public void Should_Have_Error_When_LotId_Is_Empty()
    {
        // Arrange
        var command = new AddQuantityToLotCommand
        (
            ProductId: Guid.NewGuid(),
            LotId: Guid.Empty,
            Quantity: 10
        );
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldHaveValidationErrorFor(l => l.LotId)
            .WithErrorMessage(LotErrors.NotFound.Message);
    }

    [Fact]
    public void Should_Have_Error_When_Quantity_Is_Zero()
    {
        // Arrange
        var command = new AddQuantityToLotCommand(Guid.NewGuid(), Guid.NewGuid(), 0);
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Quantity)
            .WithErrorMessage(LotErrors.NonPositiveQuantity.Message);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = new AddQuantityToLotCommand(Guid.NewGuid(),Guid.NewGuid(), 10);
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}