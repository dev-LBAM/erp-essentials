namespace ErpEssentials.Domain.Catalogs.Categories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(Guid id);
    Task<bool> IsNameUniqueAsync(string name);
}