using FluentValidation.TestHelper;
using ErpEssentials.Application.Features.Products.Create;
using ErpEssentials.Domain.Products;

namespace ErpEssentials.Application.Tests.Features.Products.Create;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Contains_Invalid_Characters()
    {
        // Arrange
        CreateProductCommand command = new() { Name = "Product Name with @" };

        // Act
        TestValidationResult<CreateProductCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(ProductErrors.InvalidNameFormat.Code);
    }
}