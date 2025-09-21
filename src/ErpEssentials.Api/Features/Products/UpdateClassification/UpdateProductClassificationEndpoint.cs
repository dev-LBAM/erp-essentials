using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Features.Products.UpdateClassification;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.UpdateClassification;

public class UpdateProductClassificationEndpoint(ISender Sender) : EndpointBase
{
    private readonly ISender _sender = Sender;
    [HttpPatch("/api/products/{id:guid}/classification", Name = ProductRoutes.UpdateClassification)]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid id, [FromBody] UpdateProductClassificationRequest request, CancellationToken cancellationToken = default)
    {
        UpdateProductClassificationCommand command = new(
            id, 
            request.NewBrandId,
            request.NewCategoryId);

        Result<ProductResponse> result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return Ok(result.Value);
    }
}