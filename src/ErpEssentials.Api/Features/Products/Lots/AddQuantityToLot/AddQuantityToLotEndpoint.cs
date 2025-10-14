using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Application.Features.Products.Lots.AddQuantityToLot;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.Lots.AddQuantityToLot;

public class AddQuantityToLotEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;
    [HttpPatch("/api/products/{productId:guid}/lots/{lotId:guid}/add-quantity", Name = LotRoutes.AddQuantityToLot)]
    [ApiExplorerSettings(GroupName = "Inventory / Lots")]
    [ProducesResponseType(typeof(LotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid productId, [FromRoute] Guid lotId, [FromBody] AddQuantityToLotRequest request, CancellationToken cancellationToken = default)
    {
        AddQuantityToLotCommand command = new(
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