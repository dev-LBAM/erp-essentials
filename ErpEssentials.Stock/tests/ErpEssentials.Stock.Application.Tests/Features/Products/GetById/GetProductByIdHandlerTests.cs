using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Application.Features.Products.GetById;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.Domain.Products.Data;
using ErpEssentials.Stock.Domain.Products.Lots;
using ErpEssentials.Stock.Infrastructure.Persistence;
using ErpEssentials.Stock.Infrastructure.Persistence.Queries;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using Microsoft.EntityFrameworkCore;


namespace ErpEssentials.Stock.Application.Tests.Features.Products.GetById;

public class GetProductByIdQueryHandlerTests
{
    private readonly AppDbContext _context;
    private readonly IProductQueries _productQueries;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        // Setup DB in memory
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        _productQueries = new ProductQueries(_context);
        _handler = new GetProductByIdQueryHandler(_productQueries);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductIsNotFound()
    {
        // Arrange
        GetProductByIdQuery query = new(Guid.NewGuid());

        // Act
        Result<ProductResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ProductErrors.NotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnCorrectProductResponse_WhenProductExists()
    {
        // Arrange
        Brand brand = Brand.Create("Nike").Value;
        Category category = Category.Create("Footwear").Value;

        CreateProductData productData = new("SKU123", "Running Shoes", null, null, 299.99m, 100m, brand.Id, category.Id);
        Product product = Product.Create(productData).Value;
        product.ReceiveStock(new CreateLotData(10, 100m, null));

        _context.Brands.Add(brand);
        _context.Categories.Add(category);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        GetProductByIdQuery query = new(product.Id);

        // Act
        Result<ProductResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        ProductResponse response = result.Value;

        Assert.Equal(product.Id, response.Id);
        Assert.Equal("SKU123", response.Sku);
        Assert.Equal("Running Shoes", response.Name);
        Assert.Equal("Nike", response.BrandName);
        Assert.Equal("Footwear", response.CategoryName);
        Assert.Equal(10, response.TotalStock);
    }
}