using System;

namespace ErpEssentials.Domain.Products.Data;

public record CreateLotData(int Quantity, decimal PurchasePrice, DateTime? ExpirationDate);