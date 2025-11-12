using ErpEssentials.Stock.Application.Contracts.Products.Lots;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Lots.GetById;

public record GetLotByIdQuery(Guid LotId) : IRequest<Result<LotResponse>>;