using ErpEssentials.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Application.Features.Catalogs.Brands.GetAllPaged;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Brands.GetAllPaged;

public class GetAllBrandsPagedQueryHandlerTests
{
    private readonly IBrandQueries _productQueries;
    private readonly Mock<IBrandQueries> _mockBrandQueries;

    private readonly GetAllBrandsPagedQueryHandler _handler;

    public GetAllBrandsPagedQueryHandlerTests()
    {
        _mockBrandQueries = new Mock<IBrandQueries>();
        _productQueries = _mockBrandQueries.Object;
        _handler = new GetAllBrandsPagedQueryHandler(_productQueries);
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedBrandResponse()
    {
        // Arrange
        GetAllBrandsPagedQuery query = new();

        PagedResult<BrandResponse> fakePagedResult = new(
            [
                new BrandResponse(
                    Guid.NewGuid(),
                    "Brand Teste"
                )
            ],
            totalItems: 1,
            page: 1,
            pageSize: 10
        );

        _mockBrandQueries
            .Setup(x => x.GetAllPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<PagedResult<BrandResponse>>.Success(fakePagedResult));

        // Act
        Result<PagedResult<BrandResponse>> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.Items);
        Assert.Equal(1, result.Value.TotalItems);
        Assert.Equal(10, result.Value.PageSize);
    }
}