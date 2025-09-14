using ErpEssentials.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Application.Features.Catalogs.Categories.Create;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;
using Moq;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Categories.Create;

public class CreateCategoryHandlerTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly Mock<ICategoryQueries> _mockCategoryQueries;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateCategoryHandler _handler;

    public CreateCategoryHandlerTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockCategoryQueries = new Mock<ICategoryQueries>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateCategoryHandler(
            _mockCategoryRepository.Object,
            _mockUnitOfWork.Object,
            _mockCategoryQueries.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenCategoryNameIsUnique()
    {
        // Arrange
        CreateCategoryCommand command = new(Name: "  drinks  ");
        string standardizedName = command.Name.ToTitleCaseStandard();

        _mockCategoryRepository
            .Setup(repo => repo.IsNameUniqueAsync(standardizedName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        Category fakeCategory = Category.Create(standardizedName).Value;

        _mockCategoryRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((b, _) => fakeCategory = b);

        _mockCategoryQueries
            .Setup(q => q.GetResponseByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CategoryResponse>.Success(new CategoryResponse
            (
                Id: fakeCategory.Id,
                Name: fakeCategory.Name
            )));

        // Act
        Result<CategoryResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Drinks", result.Value.Name);
        _mockCategoryRepository.Verify(repo =>
            repo.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _mockUnitOfWork.Verify(uow =>
            uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}