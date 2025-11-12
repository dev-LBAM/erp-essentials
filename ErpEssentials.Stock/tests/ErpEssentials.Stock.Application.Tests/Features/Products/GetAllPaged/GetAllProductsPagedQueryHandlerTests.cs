using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Application.Features.Products.GetAllPaged;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.GetAllPaged;

public class GetAllProductsPagedQueryHandlerTests
{
    private readonly IProductQueries _productQueries;
    private readonly Mock<IProductQueries> _mockProductQueries;

    private readonly GetAllProductsPagedQueryHandler _handler;

    public GetAllProductsPagedQueryHandlerTests()
    {
        _mockProductQueries = new Mock<IProductQueries>();
        _productQueries = _mockProductQueries.Object;
        _handler = new GetAllProductsPagedQueryHandler(_productQueries);
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedProductResponse()
    {
        // Arrange
        GetAllProductsPagedQuery query = new();

        PagedResult<ProductResponse> fakePagedResult = new(
            [
                new ProductResponse(
                    Guid.NewGuid(),
                    "SKU123",
                    "Produto Teste",
                    "Descrição",
                    "1234567890123",
                    100.0m,
                    50.0m,
                    "Marca Teste",
                    "Categoria Teste",
                    DateTime.UtcNow,
                    DateTime.UtcNow,
                    10,
                    true
                )
            ],
            totalItems: 1,
            page: 1,
            pageSize: 10
        );

        _mockProductQueries
            .Setup(x => x.GetAllPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<PagedResult<ProductResponse>>.Success(fakePagedResult));

        // Act
        Result<PagedResult<ProductResponse>> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.Items);
        Assert.Equal(1, result.Value.TotalItems);
        Assert.Equal(10, result.Value.PageSize);
    }
}