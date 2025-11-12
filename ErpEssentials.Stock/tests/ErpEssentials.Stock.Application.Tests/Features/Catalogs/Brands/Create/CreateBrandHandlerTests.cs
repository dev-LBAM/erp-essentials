using ErpEssentials.Stock.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Stock.Application.Features.Catalogs.Brands.Create;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using ErpEssentials.Stock.SharedKernel.Extensions;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Stock.Application.Tests.Features.Catalogs.Brands.Create;

public class CreateBrandHandlerTests
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<IBrandQueries> _mockBrandQueries;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateBrandHandler _handler;

    public CreateBrandHandlerTests()
    {
        _mockBrandRepository = new Mock<IBrandRepository>();
        _mockBrandQueries = new Mock<IBrandQueries>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateBrandHandler(
            _mockBrandRepository.Object,
            _mockUnitOfWork.Object,
            _mockBrandQueries.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenBrandNameIsUnique()
    {
        // Arrange
        CreateBrandCommand command = new ( Name : "  Puma  " );
        string standardizedName = command.Name.ToTitleCaseStandard();

        _mockBrandRepository
            .Setup(repo => repo.IsNameUniqueAsync(standardizedName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        Brand fakeBrand = Brand.Create(standardizedName).Value;

        _mockBrandRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()))
            .Callback<Brand, CancellationToken>((b, _) => fakeBrand = b);

        _mockBrandQueries
            .Setup(q => q.GetResponseByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<BrandResponse>.Success(new BrandResponse
            (
                Id: fakeBrand.Id,
                Name: fakeBrand.Name
            )));

        // Act
        Result<BrandResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Puma", result.Value.Name);
        _mockBrandRepository.Verify(repo =>
            repo.AddAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _mockUnitOfWork.Verify(uow =>
            uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}