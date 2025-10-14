namespace ErpEssentials.Domain.Products.Lots;

public interface ILotRepository
{
    Task AddAsync(Lot lot, CancellationToken cancellationToken);

    Task<Lot?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}