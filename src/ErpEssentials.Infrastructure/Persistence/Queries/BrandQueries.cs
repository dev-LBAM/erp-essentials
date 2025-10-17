
using ErpEssentials.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Infrastructure.Persistence.Queries;
public class BrandQueries(AppDbContext context) : IBrandQueries
{
    private readonly AppDbContext _context = context;
    public async Task<Result<BrandResponse>> GetResponseByIdAsync(Guid brandId, CancellationToken cancellationToken = default)
    {
        BrandResponse? brand = await _context.Brands
            .AsNoTracking()
            .Where(b => b.Id == brandId)
            .Select(b => new BrandResponse(
                b.Id,
                b.Name))
            .FirstOrDefaultAsync(cancellationToken);

        if (brand is null)
        {
            return Result<BrandResponse>.Failure(BrandErrors.NotFound);
        }

        return Result<BrandResponse>.Success(brand);
    }

    public async Task<Result<PagedResult<BrandResponse>>> GetAllPagedAsync(int page = 1, int pageSize = 10, string orderBy = "Name", bool ascending = true, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        IQueryable<Brand> query = _context.Brands.AsNoTracking();

        int totalItems = await query.CountAsync(cancellationToken);

        if (totalItems == 0)
        {
            return Result<PagedResult<BrandResponse>>.Success(new PagedResult<BrandResponse>([], totalItems, page, pageSize));
        }

        query = orderBy.ToLower() switch
        {
            "name" => ascending ? query.OrderBy(b => b.Name) : query.OrderByDescending(b => b.Name),
            _ => ascending ? query.OrderBy(b => b.Name) : query.OrderByDescending(b => b.Name),
        };

        List<BrandResponse> brands = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BrandResponse(
                b.Id,
                b.Name))
            .ToListAsync(cancellationToken);

        PagedResult<BrandResponse> pagedResult = new(brands, totalItems, page, pageSize);

        return Result<PagedResult<BrandResponse>>.Success(pagedResult);
    }
}
