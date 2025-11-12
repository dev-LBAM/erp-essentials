using Ardalis.ApiEndpoints;
using ErpEssentials.Stock.Api.Common;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Application.Features.Products.UpdateFinancials;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Stock.Api.Features.Products.UpdateFinancials;

public class UpdateProductFinancialsEndpoint(ISender Sender) : EndpointBase
{
    private readonly ISender _sender = Sender;
    [HttpPatch("/api/products/{productId:guid}/financials", Name = ProductRoutes.UpdateFinancials)]
    [ApiExplorerSettings(GroupName = "Inventory / Products")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid productId, [FromBody] UpdateProductFinancialsRequest request, CancellationToken cancellationToken = default)
    {
        UpdateProductFinancialsCommand command = new(
            productId, 
            request.NewPrice, 
            request.NewCost);

        Result<ProductResponse> result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return Ok(result.Value);
    }
}