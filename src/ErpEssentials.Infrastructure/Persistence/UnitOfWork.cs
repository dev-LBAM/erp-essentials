namespace ErpEssentials.Infrastructure.Persistence;

using ErpEssentials.SharedKernel.Abstractions;
using System.Threading;
using System.Threading.Tasks;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}