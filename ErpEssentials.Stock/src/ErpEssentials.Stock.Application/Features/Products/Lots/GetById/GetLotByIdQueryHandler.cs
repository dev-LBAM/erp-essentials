using ErpEssentials.Stock.Application.Abstractions.Products.Lots;
using ErpEssentials.Stock.Application.Contracts.Products.Lots;
using ErpEssentials.Stock.Application.Features.Products.Lots.GetById;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Lots.GetById;

public class GetLotByIdQueryHandler(ILotQueries lotQueries) : IRequestHandler<GetLotByIdQuery, Result<LotResponse>>
{
    private readonly ILotQueries _lotQueries = lotQueries;


    public async Task<Result<LotResponse>> Handle(GetLotByIdQuery request, CancellationToken cancellationToken)
    {
        return await _lotQueries.GetResponseByIdAsync(request.LotId, cancellationToken);
    }
}