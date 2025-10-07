using ErpEssentials.Domain.Products.Lots;

namespace ErpEssentials.Application.Contracts.Products.Lots;

public record LotResponse(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal PurchasePrice,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? ExpirationDate
)
{
    public static LotResponse FromEntity(Lot lot)
        => new(
            lot.Id,
            lot.ProductId,
            lot.Quantity,
            lot.PurchasePrice,
            lot.CreatedAt,
            lot.UpdatedAt ?? lot.CreatedAt,
            lot.ExpirationDate
        );
}
