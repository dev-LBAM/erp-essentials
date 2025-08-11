using ErpEssentials.Domain.Products.Lots;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Products;

public class Product
{
    private readonly List<Lot> _lots = new();

    public Guid Id { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public string Barcode { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public decimal Cost { get; private set; }
    public Guid BrandId { get; private set; }
    public Guid CategoryId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyList<Lot> Lots => _lots.AsReadOnly();

    private Product() { }

    public static Result<Product> Create(string sku, string name, string description, string? barcode, decimal price, decimal cost, Guid brandId, Guid categoryId)
    {
        List<Error> errors = new();
        string standardizedName = name.ToTitleCaseStandard();

        if (string.IsNullOrWhiteSpace(sku)) errors.Add(ProductErrors.EmptySku);
        if (string.IsNullOrWhiteSpace(standardizedName)) errors.Add(ProductErrors.EmptyName);
        if (price <= 0) errors.Add(ProductErrors.NonPositivePrice);
        if (cost < 0) errors.Add(ProductErrors.NonNegativeCost);
        if (brandId == Guid.Empty) errors.Add(ProductErrors.EmptyBrandId);
        if (categoryId == Guid.Empty) errors.Add(ProductErrors.EmptyCategoryId);

        if (errors.Any())
        {
            Dictionary<string, string[]> errorsDictionary = errors.ToDictionary(e => e.Code.Split('.').Last(), e => new[] { e.Message });
            return Result<Product>.Failure(new ValidationError(errorsDictionary));
        }

        Product product = new()
        {
            Id = Guid.NewGuid(),
            Sku = sku.Trim(),
            Barcode = barcode ?? string.Empty,
            Name = standardizedName,
            Description = description,
            Price = price,
            Cost = cost,
            BrandId = brandId,
            CategoryId = categoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return Result<Product>.Success(product);
    }

    public int GetTotalStock() => _lots.Sum(l => l.Quantity);

    public Result ReceiveStock(int quantity, decimal purchasePrice, DateTime? expirationDate)
    {
        Result<Lot> lotResult = Lot.Create(Id, quantity, purchasePrice, expirationDate);
        if (lotResult.IsFailure)
        {
            return Result.Failure(lotResult.Error);
        }
        _lots.Add(lotResult.Value);
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result RemoveStock(int quantityToRemove)
    {
        if (quantityToRemove <= 0) return Result.Failure(ProductErrors.InvalidStockQuantity);
        if (GetTotalStock() < quantityToRemove) return Result.Failure(ProductErrors.InsufficientStock);

        int quantityLeft = quantityToRemove;
        List<Lot> lotsToRemove = new();

        foreach (Lot lot in _lots.OrderBy(l => l.ExpirationDate ?? l.CreatedAt))
        {
            if (quantityLeft == 0) break;
            int removable = Math.Min(lot.Quantity, quantityLeft);
            lot.RemoveQuantity(removable);
            quantityLeft -= removable;
            if (lot.Quantity == 0) lotsToRemove.Add(lot);
        }

        lotsToRemove.ForEach(l => _lots.Remove(l));
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result AddQuantityToLot(Guid lotId, int quantityToAdd)
    {
        if (quantityToAdd <= 0)
        {
            return Result.Failure(ProductErrors.InvalidStockQuantity);
        }

        Lot? lot = _lots.FirstOrDefault(l => l.Id == lotId);
        if (lot is null)
        {
            return Result.Failure(ProductErrors.LotNotFound);
        }

        lot.AddQuantity(quantityToAdd);
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result RemoveFromSpecificLot(Guid lotId, int quantityToRemove)
    {
        if (quantityToRemove <= 0)
        {
            return Result.Failure(ProductErrors.InvalidStockQuantity);
        }

        Lot? lot = _lots.FirstOrDefault(l => l.Id == lotId);
        if (lot is null)
        {
            return Result.Failure(ProductErrors.LotNotFound);
        }

        if (lot.Quantity < quantityToRemove)
        {
            return Result.Failure(ProductErrors.InsufficientStockInLot);
        }

        lot.RemoveQuantity(quantityToRemove);

        if (lot.Quantity == 0)
        {
            _lots.Remove(lot);
        }

        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}