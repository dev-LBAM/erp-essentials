using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Lots.ReceiveStock;

public record ReceiveProductStockCommand
(
    Guid ProductId,
    int Quantity, 
    decimal PurchasePrice, 
    DateTime? ExpirationDate
) : IRequest<Result<LotResponse>>;