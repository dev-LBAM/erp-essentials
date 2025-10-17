using ErpEssentials.Application.Abstractions.Products.Lots;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Lots.GetAllPaged;

public class GetAllLotsPagedQueryHandler(ILotQueries lotQueries) : IRequestHandler<GetAllLotsPagedQuery, Result<PagedResult<LotResponse>>>
{
    private readonly ILotQueries _lotQueries = lotQueries;
    public async Task<Result<PagedResult<LotResponse>>> Handle(GetAllLotsPagedQuery request, CancellationToken cancellationToken)
    {
        return await _lotQueries.GetAllPagedAsync(request.Page, request.PageSize, request.OrderBy, request.Ascending, cancellationToken);
    }
}