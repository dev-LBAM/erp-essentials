using FluentValidation.TestHelper;
using ErpEssentials.Application.Features.Products.UpdateDetails;
using ErpEssentials.Domain.Products;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Products.UpdateDetails;

public class UpdateProductDetailsCommandValidatorTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly UpdateProductDetailsCommandValidator _validator;

    public UpdateProductDetailsCommandValidatorTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _validator = new UpdateProductDetailsCommandValidator(_mockProductRepository.Object);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        UpdateProductDetailsCommand command = new(Guid.NewGuid(), "New Valid Name", "New Desc", "UNIQUE-123");
        _mockProductRepository.Setup(r => r.IsBarcodeUniqueAsync("UNIQUE-123", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        TestValidationResult<UpdateProductDetailsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Optional_Fields_Are_Null()
    {
        // Arrange
        UpdateProductDetailsCommand command = new(Guid.NewGuid(), null, null, null);

        // Act
        TestValidationResult<UpdateProductDetailsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public async Task Should_Have_Error_When_ProductId_Is_Empty()
    {
        // Arrange
        UpdateProductDetailsCommand command = new(Guid.Empty, "Valid Name", null, null);

        // Act
        TestValidationResult<UpdateProductDetailsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ProductId)
            .WithErrorCode(ProductErrors.EmptyId.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Too_Long()
    {
        // Arrange
        string longName = new ('a', 101);
        UpdateProductDetailsCommand command = new(Guid.NewGuid(), longName, null, null);

        // Act
        TestValidationResult<UpdateProductDetailsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewName)
            .WithErrorCode(ProductErrors.NameTooLong.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Barcode_Is_Too_Long()
    {
        // Arrange
        string longBarcode = new ('1', 14);
        UpdateProductDetailsCommand command = new(Guid.NewGuid(), null, null, longBarcode);

        // Act
        TestValidationResult<UpdateProductDetailsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewBarcode)
            .WithErrorCode(ProductErrors.BarcodeTooLong.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Description_Is_Too_Long()
    {
        // Arrange
        string longDescription = new ('a', 501);
        UpdateProductDetailsCommand command = new(Guid.NewGuid(), null, longDescription, null);

        // Act
        TestValidationResult<UpdateProductDetailsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.NewDescription)
            .WithErrorCode(ProductErrors.DescriptionTooLong.Code);
    }
}