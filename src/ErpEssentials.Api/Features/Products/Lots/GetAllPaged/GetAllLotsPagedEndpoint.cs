using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Application.Features.Products.Lots.GetAllPaged;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.Lots.GetAllPaged;

public class  GetAllLotsPagedEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;

    [HttpGet("/api/lots", Name = LotRoutes.GetAllPaged)]
    [ApiExplorerSettings(GroupName = "Inventory / Lots")]
    [ProducesResponseType(typeof(PagedResult<LotResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync([FromQuery] GetAllLotsPagedQuery request, CancellationToken cancellationToken = default)
    {

        Result<PagedResult<LotResponse>> result = await _sender.Send(request, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return Ok(result.Value);
    }
}