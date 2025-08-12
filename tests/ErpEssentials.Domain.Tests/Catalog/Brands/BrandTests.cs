using Xunit;
using ErpEssentials.Domain.Catalog.Brands;
using ErpEssentials.SharedKernel.ResultPattern;
using System;

namespace ErpEssentials.Domain.Tests.Catalog.Brands;

public class BrandTests
{
    [Fact]
    public void Create_Should_ReturnSuccessResult_WhenNameIsValid()
    {
        // Arrange
        string validName = "  nike  ";

        // Act
        Result<Brand> result = Brand.Create(validName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Nike", result.Value.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Should_ReturnFailureResult_WhenNameIsInvalid(string? invalidName)
    {
        // Act
        Result<Brand> result = Brand.Create(invalidName!);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(BrandErrors.EmptyName.Code, result.Error.Code);
    }

    [Fact]
    public void UpdateName_Should_Succeed_And_StandardizeName_WhenNameIsValid()
    {
        // Arrange
        Brand brand = Brand.Create("Adidas").Value;
        string newName = "  PUMA international  ";

        // Act
        Result result = brand.UpdateName(newName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Puma International", brand.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateName_Should_Fail_WhenNewNameIsInvalid(string? invalidName)
    {
        // Arrange
        Brand brand = Brand.Create("Valid Brand").Value;

        // Act
        Result result = brand.UpdateName(invalidName!);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(BrandErrors.EmptyName.Code, result.Error.Code);
    }
}