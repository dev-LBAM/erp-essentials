using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Features.Products.UpdateDetails;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.UpdateDetails;

public class UpdateProductDetailsEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;
    [HttpPatch("/api/products/{id:guid}/details", Name = ProductRoutes.UpdateDetails)]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid id, [FromBody] UpdateProductDetailsRequest request, CancellationToken cancellationToken = default)
    {
        UpdateProductDetailsCommand command = new(id, request.NewName, request.NewDescription, request.NewBarcode);
        Result<ProductResponse> result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return Ok(result.Value);
    }
}
