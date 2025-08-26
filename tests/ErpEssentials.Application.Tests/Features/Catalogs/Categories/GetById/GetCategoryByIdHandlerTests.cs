using ErpEssentials.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Infrastructure.Persistence;
using ErpEssentials.Infrastructure.Persistence.Queries;
using ErpEssentials.Application.Features.Catalogs.Categories.GetById;
using Microsoft.EntityFrameworkCore;
using ErpEssentials.SharedKernel.ResultPattern;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Domain.Catalogs.Categories;

namespace ErpEssentials.Application.Tests.Features.Catalogs.Categories.GetById;
public class GetCategoryByIdHandlerTests
{
    private readonly AppDbContext _context;
    private readonly ICategoryQueries _categoryQueries;
    private readonly GetCategoryByIdQueryHandler _handler;
    public GetCategoryByIdHandlerTests()
    {
        // Setup DB in memory
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _categoryQueries = new CategoryQueries(_context);
        _handler = new GetCategoryByIdQueryHandler(_categoryQueries);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenCategoryIsNotFound()
    {
        // Arrange
        GetCategoryByIdQuery query = new(Guid.NewGuid());
        
        // Act
        Result<CategoryResponse> result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(CategoryErrors.NotFound.Code, result.Error.Code);
    }
    [Fact]
    public async Task Handle_Should_ReturnCorrectCategoryResponse_WhenCategoryExists()
    {
        // Arrange
        Category category = Category.Create("Footwear").Value;
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        GetCategoryByIdQuery query = new(category.Id);
        
        // Act
        Result<CategoryResponse> result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(category.Id, result.Value.Id);
        Assert.Equal(category.Name, result.Value.Name);
    }
}
