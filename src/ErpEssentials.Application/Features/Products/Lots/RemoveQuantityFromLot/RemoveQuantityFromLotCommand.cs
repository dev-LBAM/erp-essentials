using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Lots.RemoveQuantityFromLot;

public record RemoveQuantityFromLotCommand(Guid ProductId, Guid LotId, int Quantity) : IRequest<Result<LotResponse>>;
