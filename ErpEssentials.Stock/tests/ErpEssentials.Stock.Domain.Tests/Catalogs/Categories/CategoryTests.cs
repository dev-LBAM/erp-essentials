using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.SharedKernel.ResultPattern;

namespace ErpEssentials.Stock.Domain.Tests.Catalogs.Categories;

public class CategoryTests
{
    [Fact]
    public void Create_Should_ReturnSuccessResult_WhenNameIsValid()
    {
        // Arrange
        string validName = "sport shoes";

        // Act
        Result<Category> result = Category.Create(validName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Sport Shoes", result.Value.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_Should_ReturnFailureResult_WhenNameIsInvalid(string? invalidName)
    {
        // Act
        Result<Category> result = Category.Create(invalidName!);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(CategoryErrors.EmptyName.Code, result.Error.Code);
    }

    [Fact]
    public void UpdateName_Should_Succeed_And_StandardizeName_WhenNameIsValid()
    {
        // Arrange
        Category category = Category.Create("eletronics").Value;
        string newName = "  smart eletronics  ";

        // Act
        Result result = category.UpdateName(newName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Smart Eletronics", category.Name);
    }

    [Fact]
    public void UpdateName_Should_Fail_WhenNewNameIsInvalid()
    {
        // Arrange
        Category category = Category.Create("furniture").Value;
        string invalidName = "   ";

        // Act
        Result result = category.UpdateName(invalidName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(CategoryErrors.EmptyName.Code, result.Error.Code);
    }
}