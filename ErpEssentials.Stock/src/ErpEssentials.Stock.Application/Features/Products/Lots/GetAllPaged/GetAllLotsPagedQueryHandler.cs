using ErpEssentials.Stock.Application.Abstractions.Products.Lots;
using ErpEssentials.Stock.Application.Contracts.Products.Lots;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Lots.GetAllPaged;

public class GetAllLotsPagedQueryHandler(ILotQueries lotQueries) : IRequestHandler<GetAllLotsPagedQuery, Result<PagedResult<LotResponse>>>
{
    private readonly ILotQueries _lotQueries = lotQueries;
    public async Task<Result<PagedResult<LotResponse>>> Handle(GetAllLotsPagedQuery request, CancellationToken cancellationToken)
    {
        return await _lotQueries.GetAllPagedAsync(request.Page, request.PageSize, request.OrderBy, request.Ascending, cancellationToken);
    }
}