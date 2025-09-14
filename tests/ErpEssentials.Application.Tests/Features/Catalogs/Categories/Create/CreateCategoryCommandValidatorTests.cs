using ErpEssentials.Application.Features.Catalogs.Categories.Create;
using ErpEssentials.Domain.Catalogs.Categories;
using FluentValidation.TestHelper;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Categories.Create;

public class CreateCategoryCommandValidatorTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly CreateCategoryCommandValidator _validator;

    public CreateCategoryCommandValidatorTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();

        _mockCategoryRepository
            .Setup(r => r.IsNameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _validator = new CreateCategoryCommandValidator(_mockCategoryRepository.Object);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        CreateCategoryCommand command = new(Name: "A Valid Category Name");

        // Act
        TestValidationResult<CreateCategoryCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        CreateCategoryCommand command = new(Name: "");

        // Act
        TestValidationResult<CreateCategoryCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(CategoryErrors.EmptyName.Code);
    }

    [Theory]
    [InlineData("Category Name #")]
    [InlineData("Category@Name")]
    public async Task Should_Have_Error_When_Name_Contains_Invalid_Characters(string invalidName)
    {
        // Arrange
        CreateCategoryCommand command = new(Name: invalidName);

        // Act
        TestValidationResult<CreateCategoryCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(CategoryErrors.InvalidNameFormat.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        // Arrange
        string longName = new ('A', 151);
        CreateCategoryCommand command = new(Name: longName);

        // Act
        TestValidationResult<CreateCategoryCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(CategoryErrors.NameTooLong.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Not_Unique()
    {
        // Arrange
        _mockCategoryRepository
            .Setup(r => r.IsNameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        CreateCategoryCommand command = new(Name: "Existing Category");

        // Act
        TestValidationResult<CreateCategoryCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(CategoryErrors.NameInUse.Code);
    }
}