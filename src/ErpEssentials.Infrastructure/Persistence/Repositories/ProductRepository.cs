using ErpEssentials.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Infrastructure.Persistence.Repositories;

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

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // We throw this exception because the 'GetProductById' feature has not been implemented yet.
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        // We throw this exception because no 'Update' feature has been implemented yet.
        throw new NotImplementedException();
    }
}