
using ErpEssentials.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Infrastructure.Persistence.Queries;
public class CategoryQueries(AppDbContext context) : ICategoryQueries
{
    private readonly AppDbContext _context = context;
    public async Task<Result<CategoryResponse>> GetResponseByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        CategoryResponse? category = await _context.Categories
            .AsNoTracking()
            .Where(b => b.Id == categoryId)
            .Select(b => new CategoryResponse(
                b.Id,
                b.Name))
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            return Result<CategoryResponse>.Failure(CategoryErrors.NotFound);
        }

        return Result<CategoryResponse>.Success(category);
    }
    
    public async Task<Result<PagedResult<CategoryResponse>>> GetAllPagedAsync(int page = 1, int pageSize = 10, string orderBy = "Name", bool ascending = true, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        IQueryable<Category> query = _context.Categories.AsNoTracking();

        int totalItems = await query.CountAsync(cancellationToken);

        if (totalItems == 0)
        {
            return Result<PagedResult<CategoryResponse>>.Success(new PagedResult<CategoryResponse>([], totalItems, page, pageSize));
        }

        query = orderBy.ToLower() switch
        {
            "name" => ascending ? query.OrderBy(b => b.Name) : query.OrderByDescending(b => b.Name),
            _ => ascending ? query.OrderBy(b => b.Name) : query.OrderByDescending(b => b.Name),
        };

        List<CategoryResponse> categories = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new CategoryResponse(
                b.Id,
                b.Name))
            .ToListAsync(cancellationToken);

        PagedResult<CategoryResponse> pagedResult = new(categories, totalItems, page, pageSize);

        return Result<PagedResult<CategoryResponse>>.Success(pagedResult);
    }
}