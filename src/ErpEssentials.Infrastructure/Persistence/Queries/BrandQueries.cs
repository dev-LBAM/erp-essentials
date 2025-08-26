
using ErpEssentials.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Brands;
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
}
