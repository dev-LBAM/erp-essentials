using ErpEssentials.Domain.Products.Lots;

namespace ErpEssentials.Infrastructure.Persistence.Repositories;

public class LotRepository(AppDbContext context) : ILotRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Lot lot, CancellationToken cancellationToken = default)
    {
        await _context.Lots.AddAsync(lot, cancellationToken);
    }
}