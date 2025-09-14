using ErpEssentials.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Application.Features.Catalogs.Categories.UpdateName;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Categories.UpdateName;

public class UpdateCategoryNameHandlerTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly Mock<ICategoryQueries> _mockCategoryQueries;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UpdateCategoryNameHandler _handler;

    public UpdateCategoryNameHandlerTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockCategoryQueries = new Mock<ICategoryQueries>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new UpdateCategoryNameHandler(
            _mockCategoryRepository.Object,
            _mockUnitOfWork.Object,
            _mockCategoryQueries.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenCategoryNameIsUnique()
    {
        // Arrange
        UpdateCategoryNameCommand command = new(Guid.NewGuid(), "  Drinks  ");
        string standardizedName = command.NewName.ToTitleCaseStandard();

        Category fakeCategory = Category.Create(standardizedName).Value;

        _mockCategoryRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeCategory);

        _mockUnitOfWork
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockCategoryQueries
            .Setup(q => q.GetResponseByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CategoryResponse>.Success(new CategoryResponse
            (
                Id: command.CategoryId,
                Name: standardizedName
            )));

        // Act
        Result<CategoryResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Drinks", result.Value.Name);

        _mockCategoryRepository.Verify(repo =>
            repo.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()),
            Times.Once);

        _mockUnitOfWork.Verify(uow =>
            uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}