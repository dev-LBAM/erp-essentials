using ErpEssentials.Application.Features.Catalogs.Categories.UpdateName;
using ErpEssentials.Domain.Catalogs.Categories;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Categories.UpdateName;

public class UpdateCategoryNameCommandValidatorTests
{
    private readonly UpdateCategoryNameCommandValidator _validator = new();

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        UpdateCategoryNameCommand command = new(Guid.NewGuid(), "A Valid New Category Name");
        
        // Act
        TestValidationResult<UpdateCategoryNameCommand> result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_CategoryId_Is_Empty()
    {
        // Arrange
        UpdateCategoryNameCommand command = new(Guid.Empty, "A Valid New Category Name");
        
        // Act
        TestValidationResult<UpdateCategoryNameCommand> result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.CategoryId)
            .WithErrorCode(CategoryErrors.EmptyId.Code);
    }

    [Fact]
    public void Should_Have_Error_When_NewName_Is_Empty()
    {
        // Arrange
        UpdateCategoryNameCommand command = new(Guid.NewGuid(), "");
        
        // Act
        TestValidationResult<UpdateCategoryNameCommand> result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewName)
            .WithErrorCode(CategoryErrors.EmptyName.Code);
    }

    [Theory]
    [InlineData("Category Name #")]
    [InlineData("Category@Name")]
    public void Should_Have_Error_When_NewName_Contains_Invalid_Characters(string invalidName)
    {
        // Arrange
        UpdateCategoryNameCommand command = new(Guid.NewGuid(), invalidName);
        
        // Act
        TestValidationResult<UpdateCategoryNameCommand> result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewName)
            .WithErrorCode(CategoryErrors.InvalidNameFormat.Code);
    }
}
