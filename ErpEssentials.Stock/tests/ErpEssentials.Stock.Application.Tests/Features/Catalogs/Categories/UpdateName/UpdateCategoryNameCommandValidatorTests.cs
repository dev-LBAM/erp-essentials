using ErpEssentials.Stock.Application.Features.Catalogs.Categories.UpdateName;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using FluentValidation.TestHelper;
using Moq;

namespace ErpEssentials.Stock.Application.Tests.Features.Catalogs.Categories.UpdateName;

public class UpdateCategoryNameCommandValidatorTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly UpdateCategoryNameCommandValidator _validator;

    public UpdateCategoryNameCommandValidatorTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();

        _mockCategoryRepository
            .Setup(r => r.IsNameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _validator = new UpdateCategoryNameCommandValidator(_mockCategoryRepository.Object);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        UpdateCategoryNameCommand command = new(Guid.NewGuid(), "A Valid Category Name");

        // Act
        TestValidationResult<UpdateCategoryNameCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        UpdateCategoryNameCommand command = new(Guid.NewGuid(), "");

        // Act
        TestValidationResult<UpdateCategoryNameCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewName)
            .WithErrorCode(CategoryErrors.EmptyName.Code);
    }

    [Theory]
    [InlineData("Category Name #")]
    [InlineData("Category@Name")]
    public async Task Should_Have_Error_When_Name_Contains_Invalid_Characters(string invalidName)
    {
        // Arrange
        UpdateCategoryNameCommand command = new(Guid.NewGuid(), invalidName);

        // Act
        TestValidationResult<UpdateCategoryNameCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewName)
            .WithErrorCode(CategoryErrors.InvalidNameFormat.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        // Arrange
        string longName = new('A', 151);
        UpdateCategoryNameCommand command = new(Guid.NewGuid(), longName);

        // Act
        TestValidationResult<UpdateCategoryNameCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewName)
            .WithErrorCode(CategoryErrors.NameTooLong.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Not_Unique()
    {
        // Arrange
        _mockCategoryRepository
            .Setup(r => r.IsNameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        UpdateCategoryNameCommand command = new(Guid.NewGuid(), "Existing Category");

        // Act
        TestValidationResult<UpdateCategoryNameCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewName)
            .WithErrorCode(CategoryErrors.NameInUse.Code);
    }
}