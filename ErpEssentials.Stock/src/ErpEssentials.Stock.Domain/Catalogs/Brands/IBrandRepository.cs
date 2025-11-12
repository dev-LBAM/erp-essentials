namespace ErpEssentials.Stock.Domain.Catalogs.Brands;

public interface IBrandRepository
{
    Task AddAsync(Brand brand, CancellationToken cancellationToken);
    Task<Brand?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken);
}