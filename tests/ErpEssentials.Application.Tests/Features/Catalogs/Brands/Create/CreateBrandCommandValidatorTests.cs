using FluentValidation.TestHelper;
using ErpEssentials.Application.Features.Catalogs.Brands.Create;
using ErpEssentials.Domain.Catalogs.Brands;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Brands.Create;

public class CreateBrandCommandValidatorTests
{
    private readonly CreateBrandCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Contains_Invalid_Characters()
    {
        // Arrange
        CreateBrandCommand command = new() { Name = "Brand Name with #" };

        // Act
        TestValidationResult<CreateBrandCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(BrandErrors.InvalidNameFormat.Code);
    }
}