using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Features.Products.UpdateFinancials;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.UpdateFinancials;

public class UpdateProductFinancialsEndpoint(ISender Sender) : EndpointBase
{
    private readonly ISender _sender = Sender;
    [HttpPut("/api/products/{id:guid}/financials", Name = ProductRoutes.UpdateFinancials)]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid id, [FromBody] UpdateProductFinancialsRequest request, CancellationToken cancellationToken = default)
    {
        UpdateProductFinancialsCommand command = new(
            id, request.NewPrice, 
            request.NewCost);

        Result<ProductResponse> result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return Ok(result.Value);
    }
}