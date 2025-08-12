using System;

namespace ErpEssentials.Domain.Products.Lots;

public record CreateLotData(int Quantity, decimal PurchasePrice, DateTime? ExpirationDate);