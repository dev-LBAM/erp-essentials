namespace ErpEssentials.Domain.Products;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken);
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
}