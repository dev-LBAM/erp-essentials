using ErpEssentials.Application.Abstractions.Products.Lots;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Application.Features.Products.Lots.GetAllPaged;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Products.Lots.GetAllPaged;

public class GetAllLotPagedHandlerTests
{
    private readonly ILotQueries _lotQueries;
    private readonly Mock<ILotQueries> _mockLotQueries;

    private readonly GetAllLotsPagedQueryHandler _handler;

    public GetAllLotPagedHandlerTests()
    {
        _mockLotQueries = new Mock<ILotQueries>();
        _lotQueries = _mockLotQueries.Object;
        _handler = new GetAllLotsPagedQueryHandler(_lotQueries);
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedLotResponse()
    {
        // Arrange
        GetAllLotsPagedQuery query = new();

        PagedResult<LotResponse> fakePagedResult = new(
            [
                new LotResponse(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    100,
                    50.0m,
                    DateTime.UtcNow,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(6)
                )
            ],
            totalItems: 1,
            page: 1,
            pageSize: 10
        );

        _mockLotQueries
            .Setup(x => x.GetAllPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<PagedResult<LotResponse>>.Success(fakePagedResult));

        // Act
        Result<PagedResult<LotResponse>> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.Items);
        Assert.Equal(1, result.Value.TotalItems);
        Assert.Equal(10, result.Value.PageSize);
    }
}