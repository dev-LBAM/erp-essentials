using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Infrastructure.Persistence.Queries;

public class ProductQueries(AppDbContext context) : IProductQueries
{
    private readonly AppDbContext _context= context;

    public async Task<Result<ProductResponse>> GetResponseByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        ProductResponse? product = await _context.Products
        .AsNoTracking()
        .Include(p => p.Brand)
        .Include(p => p.Category)
        .Where(p => p.Id == productId)
        .Select(p => new ProductResponse(
            p.Id,
            p.Sku,
            p.Name,
            p.Description ?? string.Empty,
            p.Barcode ?? string.Empty,
            p.Price,
            p.Cost,
            p.Brand != null ? p.Brand.Name : string.Empty,
            p.Category != null ? p.Category.Name : string.Empty,
            p.CreatedAt,
            p.UpdatedAt ?? p.CreatedAt,
            p.Lots.Sum(l => (int?)l.Quantity) ?? 0
        ))
        .FirstOrDefaultAsync(cancellationToken);


        if (product is null)
        {
            return Result<ProductResponse>.Failure(ProductErrors.NotFound);
        }

        return Result<ProductResponse>.Success(product);
    }
}