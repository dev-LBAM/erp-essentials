using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Application.Features.Products.Lots.GetById;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.Lots.GetById;

public class GetLotByIdEndpoint(ISender sender) : EndpointBase
{

    private readonly ISender _sender = sender;

    [HttpGet("/api/lots/{lotId:guid}", Name = LotRoutes.GetById)]
    [ApiExplorerSettings(GroupName = "Inventory / Lots")]
    [ProducesResponseType(typeof(LotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid lotId, CancellationToken cancellationToken = default)
    {
        GetLotByIdQuery query = new(lotId);

        Result<LotResponse> result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return Ok(result.Value);
    }
}