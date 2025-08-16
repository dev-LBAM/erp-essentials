namespace ErpEssentials.Domain.Catalogs.Categories;

public interface ICategoryRepository
{
    Task AddAsync(Category category, CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid id);
    Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken);
}