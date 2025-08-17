using ErpEssentials.Domain.Catalogs.Brands;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Infrastructure.Persistence.Repositories;

public class BrandRepository(AppDbContext context) : IBrandRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Brand brand, CancellationToken cancellationToken = default)
    {
        await _context.Brands.AddAsync(brand, cancellationToken);
    }

    public async Task<Brand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Brands.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken = default)
    {
        return !await _context.Brands.AnyAsync(b => b.Name == name, cancellationToken);
    }
}