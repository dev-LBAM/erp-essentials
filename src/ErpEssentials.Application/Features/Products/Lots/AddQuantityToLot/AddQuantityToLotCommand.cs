using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Lots.AddQuantityToLot;

public record AddQuantityToLotCommand(
    Guid ProductId,
    Guid LotId,
    int Quantity) : IRequest<Result<LotResponse>>;