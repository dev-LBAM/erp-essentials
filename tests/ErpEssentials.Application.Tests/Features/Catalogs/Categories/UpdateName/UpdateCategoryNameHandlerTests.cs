using ErpEssentials.Application.Features.Catalogs.Categories.UpdateName;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Categories.UpdateName;

public class UpdateCategoryNameHandlerTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UpdateCategoryNameHandler _handler;

    public UpdateCategoryNameHandlerTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new UpdateCategoryNameHandler(
            _mockCategoryRepository.Object,
            _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenCategoryExistsAndNameIsUnique()
    {
        // Arrange
        Category existingCategory = Category.Create("Old Category Name").Value;
        Guid categoryId = existingCategory.Id;

        _mockCategoryRepository
            .Setup(repo => repo.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        _mockCategoryRepository
            .Setup(repo => repo.IsNameUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        UpdateCategoryNameCommand command = new(categoryId, "New Unique Category Name");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.True(result.IsSuccess);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenCategoryNotFound()
    {
        // Arrange
        Guid nonExistentCategoryId = Guid.NewGuid();

        _mockCategoryRepository
            .Setup(repo => repo.GetByIdAsync(nonExistentCategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);
        
        UpdateCategoryNameCommand command = new(nonExistentCategoryId, "New Category Name");
        
        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(CategoryErrors.NotFound.Code, result.Error.Code);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenNewNameIsInUseByAnotherCategory()
    {
        // Arrange
        Category existingCategory = Category.Create("Old Category Name").Value;
        Guid categoryId = existingCategory.Id;

        _mockCategoryRepository
            .Setup(repo => repo.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        _mockCategoryRepository
            .Setup(repo => repo.IsNameUniqueAsync("New Conflicting Category Name", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        UpdateCategoryNameCommand command = new(categoryId, "New Conflicting Category Name");

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(CategoryErrors.NameInUse.Code, result.Error.Code);
    }
}