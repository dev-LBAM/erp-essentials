using ErpEssentials.Domain.Catalogs.Categories;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Infrastructure.Persistence.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken = default)
    {
        return !await _context.Categories.AnyAsync(c => c.Name == name, cancellationToken);
    }
}