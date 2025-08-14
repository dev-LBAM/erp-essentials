namespace ErpEssentials.Domain.Products;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken);
    Task UpdateAsync(Product product);
}