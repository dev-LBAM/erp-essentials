using ErpEssentials.Application.Features.Products.Lots.RemoveQuantityFromLot;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Lots;
using FluentValidation.TestHelper;

namespace ErpEssentials.Application.Tests.Features.Products.Lots.RemoveQuantityFromLot;

public class RemoveQuantityFromLotCommandValidatorTests
{
    private readonly RemoveQuantityFromLotCommandValidator _validator;

    public RemoveQuantityFromLotCommandValidatorTests()
    {
        _validator = new RemoveQuantityFromLotCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        // Arrange
        var command = new RemoveQuantityFromLotCommand
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
        var command = new RemoveQuantityFromLotCommand
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
        var command = new RemoveQuantityFromLotCommand(Guid.NewGuid(), Guid.NewGuid(), 0);
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
        var command = new RemoveQuantityFromLotCommand(Guid.NewGuid(),Guid.NewGuid(), 10);
        // Act
        var result = _validator.TestValidate(command);
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}