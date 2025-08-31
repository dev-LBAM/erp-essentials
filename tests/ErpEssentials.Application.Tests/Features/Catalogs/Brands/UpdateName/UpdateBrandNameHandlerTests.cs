using ErpEssentials.Application.Features.Catalogs.Brands.UpdateName;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Brands.UpdateName;

public class UpdateBrandNameHandlerTests
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UpdateBrandNameHandler _handler;

    public UpdateBrandNameHandlerTests()
    {
        _mockBrandRepository = new Mock<IBrandRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new UpdateBrandNameHandler(
            _mockBrandRepository.Object,
            _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenBrandExistsAndNameIsUnique()
    {
        // Arrange
        Brand existingBrand = Brand.Create("Old Brand Name").Value;
        Guid brandId = existingBrand.Id;

        _mockBrandRepository
            .Setup(repo => repo.GetByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBrand);

        _mockBrandRepository
            .Setup(repo => repo.IsNameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        UpdateBrandNameCommand command = new(brandId, "New Unique Brand Name");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        _mockUnitOfWork.Verify(uow =>
            uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenBrandNotFound()
    {
        // Arrange
        Guid nonExistentBrandId = Guid.NewGuid();

        _mockBrandRepository
            .Setup(repo => repo.GetByIdAsync(nonExistentBrandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Brand?)null);

        UpdateBrandNameCommand command = new(nonExistentBrandId, "New Brand Name");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(BrandErrors.NotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenNewNameIsInUseByAnotherBrand()
    {
        // Arrange
        Brand existingBrand = Brand.Create("Old Brand Name").Value;
        Guid brandId = existingBrand.Id;

        _mockBrandRepository
            .Setup(repo => repo.GetByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBrand);

        _mockBrandRepository
            .Setup(repo => repo.IsNameUniqueAsync("New Conflicting Brand Name", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        UpdateBrandNameCommand command = new(brandId, "New Conflicting Brand Name");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(BrandErrors.NameInUse.Code, result.Error.Code);
    }
}