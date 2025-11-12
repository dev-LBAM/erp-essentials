using ErpEssentials.Stock.Application.Contracts.Products.Lots;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.ReceiveStock;

public record ReceiveProductStockCommand
(
    Guid ProductId,
    int Quantity, 
    decimal PurchasePrice, 
    DateTime? ExpirationDate
) : IRequest<Result<LotResponse>>;