using ErpEssentials.Application.Abstractions.Products.Lots;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Domain.Products.Lots;
using ErpEssentials.SharedKernel.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Infrastructure.Persistence.Queries;

public class LotQueries(AppDbContext context) : ILotQueries
{
    private readonly AppDbContext _context = context;

    public async Task<Result<LotResponse>> GetResponseByIdAsync(Guid lotId, CancellationToken cancellationToken = default)
    {
        LotResponse? lot = await _context.Lots
            .AsNoTracking()
            .Where(l => l.Id == lotId)
            .Select(l => new LotResponse(
                l.Id,
                l.ProductId,
                l.Quantity,
                l.PurchasePrice,
                l.CreatedAt,
                l.UpdatedAt ?? l.CreatedAt,
                l.ExpirationDate
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (lot is null)
        {
            return Result<LotResponse>.Failure(LotErrors.NotFound);
        }

        return Result<LotResponse>.Success(lot);
    }
}
