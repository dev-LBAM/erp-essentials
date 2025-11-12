using ErpEssentials.Stock.Application.Abstractions.Products.Lots;
using ErpEssentials.Stock.Application.Contracts.Products.Lots;
using ErpEssentials.Stock.Application.Features.Products.Lots.GetById;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.Domain.Products.Data;
using ErpEssentials.Stock.Domain.Products.Lots;
using ErpEssentials.Stock.Infrastructure.Persistence;
using ErpEssentials.Stock.Infrastructure.Persistence.Queries;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Stock.Application.Tests.Features.Products.Lots.GetById;

public class GetLotByIdQueryHandlerTests
{
    private readonly AppDbContext _context;
    private readonly ILotQueries _lotQueries;
    private readonly GetLotByIdQueryHandler _handler;

    public GetLotByIdQueryHandlerTests()
    {
        // Setup DB in memory
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        _lotQueries = new LotQueries(_context);
        _handler = new GetLotByIdQueryHandler(_lotQueries);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenLotIsNotFound()
    {
        // Arrange
        GetLotByIdQuery query = new(Guid.NewGuid());

        // Act
        Result<LotResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(LotErrors.NotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnCorrectLotResponse_WhenProductExists()
    {
        // Arrange
        Brand brand = Brand.Create("Nike").Value;
        Category category = Category.Create("Footwear").Value;

        CreateProductData productData = new("SKU123", "Running Shoes", null, null, 299.99m, 100m, brand.Id, category.Id);
        Product product = Product.Create(productData).Value;

        Result<Lot> lotResult = product.ReceiveStock(new CreateLotData(1000, 100m, null));
        Lot lot = lotResult.Value;

        _context.Brands.Add(brand);
        _context.Categories.Add(category);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        GetLotByIdQuery query = new(lot.Id);

        // Act
        Result<LotResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        LotResponse response = result.Value;
        Assert.Equal(product.Id, response.ProductId);

    }
}