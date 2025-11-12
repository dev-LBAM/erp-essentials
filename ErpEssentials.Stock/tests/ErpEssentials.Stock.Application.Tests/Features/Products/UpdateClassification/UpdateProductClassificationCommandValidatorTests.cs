using ErpEssentials.Stock.Application.Features.Products.UpdateClassification;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.Domain.Products;
using FluentValidation.TestHelper;
using Moq;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.UpdateClassification;

public class UpdateProductClassificationCommandValidatorTests
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly UpdateProductClassificationCommandValidator _validator;

    public UpdateProductClassificationCommandValidatorTests()
    {
        
        _mockBrandRepository = new Mock<IBrandRepository>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _validator = new UpdateProductClassificationCommandValidator(
            _mockBrandRepository.Object,
            _mockCategoryRepository.Object);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        Brand existingBrand = Brand.Create("Test Brand").Value;
        Category existingCategory = Category.Create("Test Category").Value;

        UpdateProductClassificationCommand command = new(Guid.NewGuid(), existingBrand.Id, existingCategory.Id);

        _mockBrandRepository
            .Setup(repo => repo.GetByIdAsync(existingBrand.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBrand);


        _mockCategoryRepository
            .Setup(repo => repo.GetByIdAsync(existingCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)existingCategory);

        // Act
        TestValidationResult<UpdateProductClassificationCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Optional_Fields_Are_Null()
    {
        // Arrange
        UpdateProductClassificationCommand command = new(Guid.NewGuid(), null, null);

        // Act
        TestValidationResult<UpdateProductClassificationCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorCode(ProductErrors.EmptyClassificationUpdate.Code);
    }


    [Fact]
    public async Task Should_Have_Error_When_ProductId_Is_Empty()
    {
        // Arrange
        UpdateProductClassificationCommand command = new(Guid.Empty, Guid.NewGuid(), Guid.NewGuid());

        // Act
        TestValidationResult<UpdateProductClassificationCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.ProductId)
            .WithErrorCode(ProductErrors.EmptyId.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_BrandId_DoesNotExist()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        _mockBrandRepository
           .Setup(repo => repo.GetByIdAsync(brandId, It.IsAny<CancellationToken>()))
           .ReturnsAsync((Brand?)null);

        _mockCategoryRepository
            .Setup(repo => repo.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Category.Create("Test Category").Value);

        UpdateProductClassificationCommand command = new(
            ProductId: Guid.NewGuid(),
            NewBrandId: brandId,
            NewCategoryId: categoryId
        );

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NewBrandId)
            .WithErrorCode(BrandErrors.NotFound.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_CategoryId_DoesNotExist()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        _mockBrandRepository
           .Setup(repo => repo.GetByIdAsync(brandId, It.IsAny<CancellationToken>()))
           .ReturnsAsync(Brand.Create("Test Category").Value);

        _mockCategoryRepository
            .Setup(repo => repo.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        UpdateProductClassificationCommand command = new(
            ProductId: Guid.NewGuid(),
            NewBrandId: brandId,
            NewCategoryId: categoryId
        );

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NewCategoryId)
            .WithErrorCode(CategoryErrors.NotFound.Code);
    }
}