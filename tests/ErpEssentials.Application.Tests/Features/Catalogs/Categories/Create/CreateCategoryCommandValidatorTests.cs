using FluentValidation.TestHelper;
using ErpEssentials.Application.Features.Catalogs.Categories.Create;
using ErpEssentials.Domain.Catalogs.Categories;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Categories.Create;

public class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Contains_Invalid_Characters()
    {
        // Arrange
        CreateCategoryCommand command = new() { Name = "Category Name with !" };

        // Act
        TestValidationResult<CreateCategoryCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(CategoryErrors.InvalidNameFormat.Code);
    }
}