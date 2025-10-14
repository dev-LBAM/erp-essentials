using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Application.Features.Products.Lots.RemoveQuantityFromLot;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.Lots.RemoveQuantityFromLot;

public class RemoveQuantityFromLotEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;
    [HttpPatch("/api/products/{productId:guid}/lots/{lotId:guid}/remove-quantity", Name = LotRoutes.RemoveQuantityFromLot)]
    [ApiExplorerSettings(GroupName = "Inventory / Lots")]
    [ProducesResponseType(typeof(LotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid productId, [FromRoute] Guid lotId, [FromBody] RemoveQuantityFromLotRequest request, CancellationToken cancellationToken = default)
    {
        RemoveQuantityFromLotCommand command = new(
            productId,
            lotId,
            request.Quantity);

        Result<LotResponse> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return Ok(result.Value);
    }
}