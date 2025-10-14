using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Application.Features.Products.RemoveStock;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.RemoveStock;

public class RemoveProductStockEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;
    [HttpPatch("/api/products/{productId:guid}/remove-stock", Name = ProductRoutes.RemoveStock)]
    [ApiExplorerSettings(GroupName = "Inventory / Products")]
    [ProducesResponseType(typeof(LotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid productId, [FromBody] RemoveProductStockRequest request, CancellationToken cancellationToken = default)
    {
        RemoveProductStockCommand command = new(
            productId,
            request.Quantity);

        Result<List<LotResponse>> result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return Ok(result.Value);
    }
}