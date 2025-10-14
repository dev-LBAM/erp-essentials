using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Api.Features.Products.Lots;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Application.Features.Products.ReceiveStock;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.ReceiveStock;

public class ReceiveProductStockEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;
    [HttpPost("/api/products/{productId:guid}/receive-stock", Name = ProductRoutes.ReceiveStock)]
    [ApiExplorerSettings(GroupName = "Inventory / Products")]
    [ProducesResponseType(typeof(LotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid productId, [FromBody] ReceiveProductStockRequest request, CancellationToken cancellationToken = default)
    {
        ReceiveProductStockCommand command = new(
            productId,
            request.Quantity,
            request.PurchasePrice,
            request.ExpirationDate);

        Result<LotResponse> result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return CreatedAtRoute(
            LotRoutes.GetById,
            new { id = result.Value.Id },
            result.Value);
    }
}
