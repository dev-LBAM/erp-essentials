using ErpEssentials.Application.Features.Catalogs.Brands.Create;
using ErpEssentials.Domain.Catalogs.Brands;
using FluentValidation.TestHelper;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Brands.Create;

public class CreateBrandCommandValidatorTests
{
    private readonly CreateBrandCommandValidator _validator = new();

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        CreateBrandCommand command = new() { Name = "A Valid Brand Name" };

        // Act
        TestValidationResult<CreateBrandCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        CreateBrandCommand command = new() { Name = "" };

        // Act
        TestValidationResult<CreateBrandCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(BrandErrors.EmptyName.Code);
    }

    [Theory]
    [InlineData("Brand Name #")]
    [InlineData("Brand@Name")]
    public void Should_Have_Error_When_Name_Contains_Invalid_Characters(string invalidName)
    {
        // Arrange
        CreateBrandCommand command = new() { Name = invalidName };

        // Act
        TestValidationResult<CreateBrandCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(BrandErrors.InvalidNameFormat.Code);
    }
}