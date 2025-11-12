using ErpEssentials.Stock.Domain.Products.Data;
using ErpEssentials.Stock.SharedKernel.ResultPattern;

namespace ErpEssentials.Stock.Domain.Products.Lots;

public class Lot
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal PurchasePrice { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? ExpirationDate { get; private set; }

    private Lot() { }

    public static Result<Lot> Create(Guid productId, CreateLotData lotData)
    {
        if (productId == Guid.Empty)
        {
            return Result<Lot>.Failure(LotErrors.MissingProductId);
        }
        if (lotData.Quantity <= 0) return Result<Lot>.Failure(LotErrors.NonPositiveQuantity);
        if (lotData.PurchasePrice < 0) return Result<Lot>.Failure(LotErrors.NonNegativePurchasePrice);

        Lot lot = new()
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Quantity = lotData.Quantity,
            PurchasePrice = lotData.PurchasePrice,
            ExpirationDate = lotData.ExpirationDate,
            CreatedAt = DateTime.UtcNow
        };

        return Result<Lot>.Success(lot);
    }

    internal Result<Lot> AddQuantity(int amount)
    {
        if (amount <= 0) return Result<Lot>.Failure(LotErrors.NonPositiveQuantity);
        Quantity += amount;
        UpdatedAt = DateTime.UtcNow;
        return Result<Lot>.Success(this);
    }

    internal Result<Lot> RemoveQuantity(int amount)
    {
        if (amount <= 0) return Result<Lot>.Failure(LotErrors.NonPositiveQuantity);
        if (amount > Quantity) return Result<Lot>.Failure(LotErrors.InsufficientStockInLot);
        Quantity -= amount;
        UpdatedAt = DateTime.UtcNow;
        return Result<Lot>.Success(this);
    }
}