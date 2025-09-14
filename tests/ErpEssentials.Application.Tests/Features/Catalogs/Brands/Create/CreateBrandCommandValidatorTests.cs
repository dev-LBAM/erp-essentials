using ErpEssentials.Application.Features.Catalogs.Brands.Create;
using ErpEssentials.Domain.Catalogs.Brands;
using FluentValidation.TestHelper;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Brands.Create;

public class CreateBrandCommandValidatorTests
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly CreateBrandCommandValidator _validator;

    public CreateBrandCommandValidatorTests()
    {
        _mockBrandRepository = new Mock<IBrandRepository>();

        _mockBrandRepository
            .Setup(r => r.IsNameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _validator = new CreateBrandCommandValidator(_mockBrandRepository.Object);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        CreateBrandCommand command = new(Name: "A Valid Brand Name");

        // Act
        TestValidationResult<CreateBrandCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        CreateBrandCommand command = new(Name: "");

        // Act
        TestValidationResult<CreateBrandCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(BrandErrors.EmptyName.Code);
    }

    [Theory]
    [InlineData("Brand Name #")]
    [InlineData("Brand@Name")]
    public async Task Should_Have_Error_When_Name_Contains_Invalid_Characters(string invalidName)
    {
        // Arrange
        CreateBrandCommand command = new(Name: invalidName);

        // Act
        TestValidationResult<CreateBrandCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(BrandErrors.InvalidNameFormat.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        // Arrange
        string longName = new ('A', 101);
        CreateBrandCommand command = new(Name: longName);

        // Act
        TestValidationResult<CreateBrandCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(BrandErrors.NameTooLong.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Not_Unique()
    {
        // Arrange
        _mockBrandRepository
            .Setup(r => r.IsNameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        CreateBrandCommand command = new(Name: "Existing Brand");

        // Act
        TestValidationResult<CreateBrandCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(BrandErrors.NameInUse.Code);
    }
}