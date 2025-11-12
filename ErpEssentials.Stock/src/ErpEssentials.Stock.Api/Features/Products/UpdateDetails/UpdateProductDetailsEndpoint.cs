using Ardalis.ApiEndpoints;
using ErpEssentials.Stock.Api.Common;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Application.Features.Products.UpdateDetails;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Stock.Api.Features.Products.UpdateDetails;

public class UpdateProductDetailsEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;
    [HttpPatch("/api/products/{productId:guid}/details", Name = ProductRoutes.UpdateDetails)]
    [ApiExplorerSettings(GroupName = "Inventory / Products")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid productId, [FromBody] UpdateProductDetailsRequest request, CancellationToken cancellationToken = default)
    {
        UpdateProductDetailsCommand command = new(productId, request.NewName, request.NewDescription, request.NewBarcode);
        Result<ProductResponse> result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return Ok(result.Value);
    }
}
