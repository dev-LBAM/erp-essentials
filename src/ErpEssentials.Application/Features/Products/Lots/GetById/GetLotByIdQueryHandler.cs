using ErpEssentials.Application.Abstractions.Products.Lots;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Application.Features.Products.Lots.GetById;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Lots.GetById;

public class GetLotByIdQueryHandler(ILotQueries lotQueries) : IRequestHandler<GetLotByIdQuery, Result<LotResponse>>
{
    private readonly ILotQueries _lotQueries = lotQueries;


    public async Task<Result<LotResponse>> Handle(GetLotByIdQuery request, CancellationToken cancellationToken)
    {
        return await _lotQueries.GetResponseByIdAsync(request.LotId, cancellationToken);
    }
}