using Moq;
using ErpEssentials.Application.Features.Catalogs.Brands.Create;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.SharedKernel.ResultPattern;
using ErpEssentials.SharedKernel.Extensions;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Brands.Create;

public class CreateBrandHandlerTests
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly CreateBrandHandler _handler;

    public CreateBrandHandlerTests()
    {
        _mockBrandRepository = new Mock<IBrandRepository>();
        _handler = new CreateBrandHandler(_mockBrandRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenBrandNameIsNotUnique()
    {
        // Arrange
        CreateBrandCommand command = new() { Name = "  Nike  " };
        string standardizedName = command.Name.ToTitleCaseStandard();

        _mockBrandRepository
            .Setup(repo => repo.IsNameUniqueAsync(standardizedName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        Result<Brand> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(BrandErrors.NameInUse.Code, result.Error.Code);
        _mockBrandRepository.Verify(repo =>
            repo.AddAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenBrandNameIsUnique()
    {
        // Arrange
        CreateBrandCommand command = new() { Name = "  Puma  " };
        string standardizedName = command.Name.ToTitleCaseStandard();

        _mockBrandRepository
            .Setup(repo => repo.IsNameUniqueAsync(standardizedName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Result<Brand> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Puma", result.Value.Name);
        _mockBrandRepository.Verify(repo =>
            repo.AddAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}