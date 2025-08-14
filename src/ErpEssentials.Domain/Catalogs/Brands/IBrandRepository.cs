namespace ErpEssentials.Domain.Catalogs.Brands;

public interface IBrandRepository
{
    Task AddAsync(Brand brand);
    Task<Brand?> GetByIdAsync(Guid id);
    Task<bool> IsNameUniqueAsync(string name);
}