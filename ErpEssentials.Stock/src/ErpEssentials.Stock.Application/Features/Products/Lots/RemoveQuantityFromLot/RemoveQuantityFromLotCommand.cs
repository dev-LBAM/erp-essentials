using ErpEssentials.Stock.Application.Contracts.Products.Lots;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Lots.RemoveQuantityFromLot;

public record RemoveQuantityFromLotCommand(Guid ProductId, Guid LotId, int Quantity) : IRequest<Result<LotResponse>>;
