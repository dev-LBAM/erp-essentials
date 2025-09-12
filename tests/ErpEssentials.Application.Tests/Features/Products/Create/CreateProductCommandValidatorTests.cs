using Xunit;
using FluentValidation.TestHelper;
using ErpEssentials.Application.Features.Products.Create;
using ErpEssentials.Domain.Products;
using System;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.Application.Features.Catalogs.Brands.Create;
using ErpEssentials.Application.Features.Catalogs.Categories.Create;

namespace ErpEssentials.Application.Tests.Features.Products.Create;

public class CreateProductCommandValidatorTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;


    private readonly CreateProductCommandValidator _validator;

    public CreateProductCommandValidatorTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockBrandRepository = new Mock<IBrandRepository>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();

        _validator = new CreateProductCommandValidator(
            _mockProductRepository.Object,
            _mockBrandRepository.Object,
            _mockCategoryRepository.Object);
    }

    private static CreateProductCommand CreateValidCommand() => new(
        Sku: "VALID-SKU",
        Name: "Valid Name",
        Description: "Valid Description",
        Barcode: "VALID-BC",
        Price: 100m,
        Cost: 50m,
        BrandId: Guid.NewGuid(),
        CategoryId: Guid.NewGuid()
    );

    [Fact]
    public async Task Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        CreateProductCommand command = CreateValidCommand();
        Brand brand = Brand.Create("A Valid Brand Name").Value;
        Category category = Category.Create("A Valid Category Name").Value;

        _mockProductRepository.Setup(r => r.IsBarcodeUniqueAsync(command.Barcode!, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockProductRepository.Setup(r => r.GetBySkuAsync(command.Sku, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);
        _mockBrandRepository.Setup(r => r.GetByIdAsync(brand.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Brand?)brand);
        _mockCategoryRepository.Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Category?)category);
        
        // Act
        TestValidationResult<CreateProductCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Should_Have_Error_When_Sku_Is_Empty()
    {
        // Arrange
        CreateProductCommand command = CreateValidCommand() with { Sku = string.Empty };

        // Act
        TestValidationResult<CreateProductCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Sku)
            .WithErrorCode(ProductErrors.EmptySku.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Too_Long()
    {
        // Arrange
        string longName = new ('a', 101);
        CreateProductCommand command = CreateValidCommand() with { Name = longName };

        // Act
        TestValidationResult<CreateProductCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorCode(ProductErrors.NameTooLong.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Barcode_Is_Not_Unique()
    {
        // Arrange
        CreateProductCommand command = CreateValidCommand() with { Barcode = "DUPLICATE-BC" };
        _mockProductRepository
            .Setup(repo => repo.IsBarcodeUniqueAsync("DUPLICATE-BC", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        TestValidationResult<CreateProductCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Barcode)
            .WithErrorCode(ProductErrors.BarcodeInUse.Code);
    }

    [Fact]
    public async Task Should_Have_Error_When_Price_Is_Zero()
    {
        // Arrange
        CreateProductCommand command = CreateValidCommand() with { Price = 0 };

        // Act
        TestValidationResult<CreateProductCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Price)
            .WithErrorCode(ProductErrors.NonPositivePrice.Code);
    }
}