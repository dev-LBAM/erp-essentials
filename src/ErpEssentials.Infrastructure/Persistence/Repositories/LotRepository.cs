using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Lots;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Infrastructure.Persistence.Repositories;

public class LotRepository(AppDbContext context) : ILotRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Lot lot, CancellationToken cancellationToken = default)
    {
        await _context.Lots.AddAsync(lot, cancellationToken);
    }

    public async Task<Lot?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Lots.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}