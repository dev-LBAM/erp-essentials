using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.SharedKernel.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Stock.Infrastructure.Persistence.Repositories;

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
        string standardizedName = name.ToTitleCaseStandard();
        return !await _context.Brands.AnyAsync(b => b.Name == standardizedName, cancellationToken);
    }
}