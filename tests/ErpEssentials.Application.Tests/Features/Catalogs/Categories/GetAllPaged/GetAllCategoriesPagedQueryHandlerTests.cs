using ErpEssentials.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Application.Features.Catalogs.Categories.GetAllPaged;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Categories.GetAllPaged;

public class GetAllCategoriesPagedHandlerTests
{
    private readonly ICategoryQueries _productQueries;
    private readonly Mock<ICategoryQueries> _mockCategoryQueries;

    private readonly GetAllCategoriesPagedQueryHandler _handler;

    public GetAllCategoriesPagedHandlerTests()
    {
        _mockCategoryQueries = new Mock<ICategoryQueries>();
        _productQueries = _mockCategoryQueries.Object;
        _handler = new GetAllCategoriesPagedQueryHandler(_productQueries);
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedCategoryResponse()
    {
        // Arrange
        GetAllCategoriesPagedQuery query = new();

        PagedResult<CategoryResponse> fakePagedResult = new(
            [
                new CategoryResponse(
                    Guid.NewGuid(),
                    "Categoria Teste"
                )
            ],
            totalItems: 1,
            page: 1,
            pageSize: 10
        );

        _mockCategoryQueries
            .Setup(x => x.GetAllPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<PagedResult<CategoryResponse>>.Success(fakePagedResult));

        // Act
        Result<PagedResult<CategoryResponse>> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.Items);
        Assert.Equal(1, result.Value.TotalItems);
        Assert.Equal(10, result.Value.PageSize);
    }
}