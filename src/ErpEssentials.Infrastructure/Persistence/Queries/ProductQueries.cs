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
            .Where(p => p.Id == productId)
            .Select(p => new ProductResponse(
                p.Id, 
                p.Sku, 
                p.Name, 
                p.Description,
                p.Barcode,
                p.Price,
                p.Cost,
                p.Brand!.Name ?? "",
                p.Category!.Name ?? "",
                p.CreatedAt,
                p.UpdatedAt,
                p.Lots.Sum(l => l.Quantity)
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            return Result<ProductResponse>.Failure(ProductErrors.NotFound);
        }

        return Result<ProductResponse>.Success(product);
    }
}