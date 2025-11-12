using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Stock.Infrastructure.Persistence.Queries;

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
            p.Lots.Sum(l => (int?)l.Quantity) ?? 0,
            p.IsActive
        ))
        .FirstOrDefaultAsync(cancellationToken);


        if (product is null)
        {
            return Result<ProductResponse>.Failure(ProductErrors.NotFound);
        }

        return Result<ProductResponse>.Success(product);
    }

    public async Task<Result<PagedResult<ProductResponse>>> GetAllPagedAsync(int page = 1, int pageSize = 10, string orderBy = "Name", bool ascending = true, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;
        IQueryable<Product> query = _context.Products
        .AsNoTracking()
        .Include(p => p.Brand)
        .Include(p => p.Category);

        // Dynamic ordering
        query = orderBy.ToLower() switch
        {
            "name" => ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
            "price" => ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
            "cost" => ascending ? query.OrderBy(p => p.Cost) : query.OrderByDescending(p => p.Cost),
            "createdat" => ascending ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt),
            _ => ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name)
        };

        int totalItems = await query.CountAsync(cancellationToken);

        List<ProductResponse> products = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
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
            p.Lots.Sum(l => (int?)l.Quantity) ?? 0,
            p.IsActive
        ))
        .ToListAsync(cancellationToken);

        PagedResult<ProductResponse> pagedResult = new(products, totalItems, page, pageSize);

        return Result<PagedResult<ProductResponse>>.Success(pagedResult);
    }
}