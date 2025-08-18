using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.GetById;

public class GetProductByIdEndpoint : EndpointBaseAsync
    .WithRequest<Guid>
    .WithoutResult
{
    [HttpGet("/api/products/{id:guid}", Name = ProductRoutes.GetById)]
    public override Task<ActionResult> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // TODO: Implement the real logic by creating and sending a GetProductByIdQuery.

        ActionResult result = StatusCode(StatusCodes.Status501NotImplemented, "This feature is not yet implemented.");

        return Task.FromResult(result);
    }
}