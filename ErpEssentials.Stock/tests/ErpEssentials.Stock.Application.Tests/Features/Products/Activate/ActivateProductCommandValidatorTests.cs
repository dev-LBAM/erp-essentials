using ErpEssentials.Stock.Application.Features.Products.Activate;
using ErpEssentials.Stock.Domain.Products;
using FluentValidation.TestHelper;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.Activate;

public class ActivateProductCommandValidatorTests
{
    private readonly ActivateProductCommandValidator _validator;
    public ActivateProductCommandValidatorTests()
    {
        _validator = new ActivateProductCommandValidator();
    }
    [Fact]
    public async Task Validate_ShouldHaveError_WhenProductIdIsEmpty()
    {
        // Arrange
        ActivateProductCommand command = new (Guid.Empty);

        // Act
        TestValidationResult<ActivateProductCommand> result = await _validator.TestValidateAsync(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(p => p.ProductId)
            .WithErrorCode(ProductErrors.EmptyId.Code);
    }
    [Fact]
    public async Task Validate_ShouldNotHaveError_WhenProductIdIsValid()
    {
        // Arrange
        ActivateProductCommand command = new(Guid.NewGuid());
        
        // Act
        TestValidationResult<ActivateProductCommand> result = await _validator.TestValidateAsync(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}