
using System.Reflection.Metadata;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Application.Features.Products.UpdateDetails;
using ErpEssentials.Stock.Application.Features.Products.UpdateFinancials;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.Domain.Products.Data;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using FluentValidation.TestHelper;
using Moq;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.UpdateFinancials;

public class UpdateProductFinancialsCommandValidatorTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly UpdateProductFinancialsCommandValidator _validator;

    public UpdateProductFinancialsCommandValidatorTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _validator = new UpdateProductFinancialsCommandValidator();
    }

    private static Product CreateTestProduct()
    {
        Brand brand = Brand.Create("Test Brand").Value;
        Category category = Category.Create("Test Category").Value;
        CreateProductData initialData = new("SKU", "Old Name", "Old Desc", "Old Barcode", 100, 50, brand.Id, category.Id);
        Product product = Product.Create(initialData).Value;

        typeof(Product).GetProperty(nameof(Product.Brand))?.SetValue(product, brand, null);
        typeof(Product).GetProperty(nameof(Product.Category))?.SetValue(product, category, null);
        return product;
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        UpdateProductFinancialsCommand command = new(Guid.NewGuid(), 15m, 5m);

        // Act
        TestValidationResult<UpdateProductFinancialsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Optional_Fields_Are_Null()
    {
        // Arrange
        UpdateProductFinancialsCommand command = new(Guid.NewGuid(), null, null);

        // Act
        TestValidationResult<UpdateProductFinancialsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorCode(ProductErrors.EmptyFinancialUpdate.Code);
    }


    [Fact]
    public async Task Should_Have_Error_When_ProductId_Is_Empty()
    {
        // Arrange
        UpdateProductFinancialsCommand command = new(Guid.Empty, 15m, 5m);

        // Act
        TestValidationResult<UpdateProductFinancialsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.ProductId)
            .WithErrorCode(ProductErrors.EmptyId.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPriceIsNotGreatherThan0()
    {
        // Arrange
        Product product = CreateTestProduct();
        UpdateProductFinancialsCommand command = new(ProductId: product.Id, NewPrice: 0m, NewCost: 1m);
        
        TestValidationResult<UpdateProductFinancialsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.NewPrice)
            .WithErrorCode(ProductErrors.NonPositivePrice.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenCostIsNotGreaterThanOrEqualTo0()
    {
        // Arrange
        Product product = CreateTestProduct();
        UpdateProductFinancialsCommand command = new(ProductId: product.Id, NewPrice: 1m, NewCost: -1m);

        TestValidationResult<UpdateProductFinancialsCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.NewCost)
            .WithErrorCode(ProductErrors.NonNegativeCost.Code);
    }
}
