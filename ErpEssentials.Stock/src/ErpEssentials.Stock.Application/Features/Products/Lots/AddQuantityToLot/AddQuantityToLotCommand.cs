using ErpEssentials.Stock.Application.Contracts.Products.Lots;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Lots.AddQuantityToLot;

public record AddQuantityToLotCommand(
    Guid ProductId,
    Guid LotId,
    int Quantity) : IRequest<Result<LotResponse>>;