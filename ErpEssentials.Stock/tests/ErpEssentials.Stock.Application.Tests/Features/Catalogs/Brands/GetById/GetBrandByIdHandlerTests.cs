using ErpEssentials.Stock.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.Application.Features.Catalogs.Brands.GetById;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Infrastructure.Persistence;
using ErpEssentials.Stock.Infrastructure.Persistence.Queries;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Stock.Application.Tests.Features.Catalogs.Brands.GetById;

public class GetBrandByIdHandlerTests
{
    private readonly AppDbContext _context;
    private readonly IBrandQueries _brandQueries;
    private readonly GetBrandByIdQueryHandler _handler;

    public GetBrandByIdHandlerTests()
    {
        // Setup DB in memory
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _brandQueries = new BrandQueries(_context);
        _handler = new GetBrandByIdQueryHandler(_brandQueries);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenBrandIsNotFound()
    {
        // Arrange
        GetBrandByIdQuery query = new(Guid.NewGuid());
        
        // Act
        Result<BrandResponse> result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(BrandErrors.NotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnCorrectBrandResponse_WhenBrandExists()
    {
        // Arrange
        Brand brand = Brand.Create("Nike").Value;
        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();
        GetBrandByIdQuery query = new(brand.Id);
        
        // Act
        Result<BrandResponse> result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(brand.Id, result.Value.Id);
        Assert.Equal(brand.Name, result.Value.Name);
    }
}
