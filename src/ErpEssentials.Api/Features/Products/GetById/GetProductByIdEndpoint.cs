using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Features.Products.GetById;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.GetById;

public class GetProductByIdEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<ProductResponse>
{

    private readonly ISender _sender = sender;

    [HttpGet("/api/products/{productId:guid}", Name = ProductRoutes.GetById)]
    [ApiExplorerSettings(GroupName = "Inventory / Products")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public override async Task<ActionResult<ProductResponse>> HandleAsync([FromRoute]Guid productId, CancellationToken cancellationToken = default)
    {
        GetProductByIdQuery query = new(productId);

        Result<ProductResponse> result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return Ok(result.Value);
    }
}