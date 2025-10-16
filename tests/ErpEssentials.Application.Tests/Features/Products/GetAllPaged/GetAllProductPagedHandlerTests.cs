using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Features.Products.GetAllPaged;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Products.GetAllPaged;

public class GetAllProductPagedHandlerTests
{
    private readonly IProductQueries _productQueries;
    private readonly Mock<IProductQueries> _mockProductQueries;

    private readonly GetAllProductPageQueryHandler _handler;

    public GetAllProductPagedHandlerTests()
    {
        _mockProductQueries = new Mock<IProductQueries>();
        _productQueries = _mockProductQueries.Object;
        _handler = new GetAllProductPageQueryHandler(_productQueries);
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedProductResponse()
    {
        // Arrange
        GetAllProductPagedQuery query = new();

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