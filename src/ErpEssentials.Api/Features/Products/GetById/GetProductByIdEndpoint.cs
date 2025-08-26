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

    [HttpGet("/api/products/{id:guid}", Name = ProductRoutes.GetById)]
    public override async Task<ActionResult<ProductResponse>> HandleAsync([FromRoute]Guid id, CancellationToken cancellationToken = default)
    {
        GetProductByIdQuery query = new(id);

        Result<ProductResponse> result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return Ok(result.Value);
    }
}