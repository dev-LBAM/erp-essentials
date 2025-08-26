
using ErpEssentials.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Domain.Catalogs.Categories;
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
}
