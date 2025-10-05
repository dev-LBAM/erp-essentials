using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Lots.GetById;

public record GetLotByIdQuery(Guid LotId) : IRequest<Result<LotResponse>>;