using ErpEssentials.Stock.Application.Features.Products.Deactivate;
using ErpEssentials.Stock.Domain.Products;
using FluentValidation.TestHelper;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.Deactivate;

public class DeactivateProductCommandValidatorTests
{
    private readonly DeactivateProductCommandValidator _validator;
    public DeactivateProductCommandValidatorTests()
    {
        _validator = new DeactivateProductCommandValidator();
    }
    [Fact]
    public async Task Validate_ShouldHaveError_WhenProductIdIsEmpty()
    {
        // Arrange
        DeactivateProductCommand command = new (Guid.Empty);

        // Act
        TestValidationResult<DeactivateProductCommand> result = await _validator.TestValidateAsync(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(p => p.ProductId)
            .WithErrorCode(ProductErrors.EmptyId.Code);
    }
    [Fact]
    public async Task Validate_ShouldNotHaveError_WhenProductIdIsValid()
    {
        // Arrange
        DeactivateProductCommand command = new(Guid.NewGuid());
        
        // Act
        TestValidationResult<DeactivateProductCommand> result = await _validator.TestValidateAsync(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}