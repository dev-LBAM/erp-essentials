using ErpEssentials.Application.Abstractions.Products.Lots;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Domain.Products.Lots;
using ErpEssentials.SharedKernel.Pagination;
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

    public async Task<Result<PagedResult<LotResponse>>> GetAllPagedAsync(int page = 1, int pageSize = 10, string orderBy = "CreateAt", bool ascending = true, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;
        IQueryable<Lot> query = _context.Lots.AsNoTracking();
        int totalItems = await query.CountAsync(cancellationToken);

        if (totalItems == 0)
        {
            return Result<PagedResult<LotResponse>>.Success(new PagedResult<LotResponse>([], totalItems, page, pageSize));
        }

        query = orderBy.ToLower() switch
        {
            "createdat" or "createat" or "created" => ascending ? query.OrderBy(l => l.CreatedAt) : query.OrderByDescending(l => l.CreatedAt),
            "updatedat" or "updateat" or "updated" => ascending ? query.OrderBy(l => l.UpdatedAt) : query.OrderByDescending(l => l.UpdatedAt),
            "expirationdate" or "expiration" => ascending ? query.OrderBy(l => l.ExpirationDate) : query.OrderByDescending(l => l.ExpirationDate),
            "quantity" => ascending ? query.OrderBy(l => l.Quantity) : query.OrderByDescending(l => l.Quantity),
            "purchaseprice" or "price" => ascending ? query.OrderBy(l => l.PurchasePrice) : query.OrderByDescending(l => l.PurchasePrice),
            _ => ascending ? query.OrderBy(l => l.CreatedAt) : query.OrderByDescending(l => l.CreatedAt),
        };

        List<LotResponse> lots = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new LotResponse(
                l.Id,
                l.ProductId,
                l.Quantity,
                l.PurchasePrice,
                l.CreatedAt,
                l.UpdatedAt ?? l.CreatedAt,
                l.ExpirationDate
            ))
            .ToListAsync(cancellationToken);

        PagedResult<LotResponse> pagedResult = new(lots, totalItems, page, pageSize);

        return Result<PagedResult<LotResponse>>.Success(pagedResult);
    }
}