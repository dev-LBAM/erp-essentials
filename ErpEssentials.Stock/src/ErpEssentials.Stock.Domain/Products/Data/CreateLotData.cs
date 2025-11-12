using System;

namespace ErpEssentials.Stock.Domain.Products.Data;

public record CreateLotData(int Quantity, decimal PurchasePrice, DateTime? ExpirationDate);