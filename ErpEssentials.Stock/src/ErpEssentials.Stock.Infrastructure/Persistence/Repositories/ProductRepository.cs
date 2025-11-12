using ErpEssentials.Stock.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Stock.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
        .Include(p => p.Brand)
        .Include(p => p.Category)
        .Include(p => p.Lots)
        .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> IsBarcodeUniqueAsync(string barcode, CancellationToken cancellationToken = default)
    {
        return !await _context.Products.AnyAsync(p => p.Barcode == barcode, cancellationToken);
    }
}