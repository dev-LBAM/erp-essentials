using ErpEssentials.Stock.Application.Features.Catalogs.Brands.UpdateName;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using FluentValidation.TestHelper;
using Moq;

namespace ErpEssentials.Stock.Application.Tests.Features.Catalogs.Brands.UpdateName;

public class UpdateBrandNameCommandValidatorTests
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly UpdateBrandNameCommandValidator _validator;

    public UpdateBrandNameCommandValidatorTests()
    {
        _mockBrandRepository = new Mock<IBrandRepository>();

        _mockBrandRepository
            .Setup(r => r.IsNameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _validator = new UpdateBrandNameCommandValidator(_mockBrandRepository.Object);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        UpdateBrandNameCommand command = new(Guid.NewGuid(), "A Valid Brand Name");

        // Act
        TestValidationResult<UpdateBrandNameCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        UpdateBrandNameCommand command = new(Guid.NewGuid(), "");

        // Act
        TestValidationResult<UpdateBrandNameCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.NewName)
            .WithErrorCode(BrandErrors.EmptyName.Code);
    }

    [Theory]
    [InlineData("Brand Name #")]
    [InlineData("Brand@Name")]
    public async Task Should_Have_Error_When_Name_Contains_Invalid_Characters(string invalidName)
    {
        // Arrange
        UpdateBrandNameCommand command = new(Guid.NewGuid(), invalidName);

        // Act
        TestValidationResult<UpdateBrandNameCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewName)
            .WithErrorCode(BrandErrors.InvalidNameFormat.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        // Arrange
        string longName = new('A', 101);
        UpdateBrandNameCommand command = new(Guid.NewGuid(), longName);

        // Act
        TestValidationResult<UpdateBrandNameCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.NewName)
            .WithErrorCode(BrandErrors.NameTooLong.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Not_Unique()
    {
        // Arrange
        _mockBrandRepository
            .Setup(r => r.IsNameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        UpdateBrandNameCommand command = new(Guid.NewGuid(), "Existing Brand");

        // Act
        TestValidationResult<UpdateBrandNameCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.NewName)
            .WithErrorCode(BrandErrors.NameInUse.Code);
    }
}