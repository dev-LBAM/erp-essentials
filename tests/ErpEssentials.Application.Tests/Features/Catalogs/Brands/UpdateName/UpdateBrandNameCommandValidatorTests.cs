using FluentValidation.TestHelper;
using ErpEssentials.Application.Features.Catalogs.Brands.UpdateName;
using ErpEssentials.Domain.Catalogs.Brands;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Brands.UpdateName;

public class UpdateBrandNameCommandValidatorTests
{
    private readonly UpdateBrandNameCommandValidator _validator = new();

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        UpdateBrandNameCommand command = new(Guid.NewGuid(), "A Valid New Brand Name");

        // Act
        TestValidationResult<UpdateBrandNameCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_BrandId_Is_Empty()
    {
        // Arrange
        UpdateBrandNameCommand command = new(Guid.Empty, "A Valid New Brand Name");

        // Act
        TestValidationResult<UpdateBrandNameCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.BrandId)
            .WithErrorCode(BrandErrors.EmptyId.Code);
    }

    [Fact]
    public void Should_Have_Error_When_NewName_Is_Empty()
    {
        // Arrange
        UpdateBrandNameCommand command = new(Guid.NewGuid(), "");

        // Act
        TestValidationResult<UpdateBrandNameCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewName)
            .WithErrorCode(BrandErrors.EmptyName.Code);
    }

    [Theory]
    [InlineData("Brand Name #")]
    [InlineData("Brand@Name")]
    public void Should_Have_Error_When_NewName_Contains_Invalid_Characters(string invalidName)
    {
        // Arrange
        UpdateBrandNameCommand command = new(Guid.NewGuid(), invalidName);

        // Act
        TestValidationResult<UpdateBrandNameCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewName)
            .WithErrorCode(BrandErrors.InvalidNameFormat.Code);
    }
}