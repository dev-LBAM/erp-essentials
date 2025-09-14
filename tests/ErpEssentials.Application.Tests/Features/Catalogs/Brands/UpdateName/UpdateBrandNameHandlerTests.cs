using ErpEssentials.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Application.Features.Catalogs.Brands.UpdateName;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Brands.UpdateName;

public class UpdateBrandNameHandlerTests
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<IBrandQueries> _mockBrandQueries;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UpdateBrandNameHandler _handler;

    public UpdateBrandNameHandlerTests()
    {
        _mockBrandRepository = new Mock<IBrandRepository>();
        _mockBrandQueries = new Mock<IBrandQueries>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new UpdateBrandNameHandler(
            _mockBrandRepository.Object,
            _mockUnitOfWork.Object,
            _mockBrandQueries.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenBrandNameIsUnique()
    {
        // Arrange
        UpdateBrandNameCommand command = new(Guid.NewGuid(), "  Puma  ");
        string standardizedName = command.NewName.ToTitleCaseStandard();

        Brand fakeBrand = Brand.Create(standardizedName).Value;

        _mockBrandRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeBrand);

        _mockUnitOfWork
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockBrandQueries
            .Setup(q => q.GetResponseByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<BrandResponse>.Success(new BrandResponse
            (
                Id: command.BrandId,
                Name: standardizedName
            )));

        // Act
        Result<BrandResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Puma", result.Value.Name);

        _mockBrandRepository.Verify(repo =>
            repo.GetByIdAsync(command.BrandId, It.IsAny<CancellationToken>()),
            Times.Once);

        _mockUnitOfWork.Verify(uow =>
            uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

}