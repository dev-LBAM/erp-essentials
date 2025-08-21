using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Catalogs.Brands.GetById;

public class GetBrandByIdEndpoint : EndpointBaseAsync
    .WithRequest<Guid>
    .WithoutResult
{
    [HttpGet("/api/brands/{id:guid}", Name = BrandRoutes.GetById)]
    public override Task<ActionResult> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // TODO: Implement the real logic by creating and sending a GetProductByIdQuery.

        ActionResult result = StatusCode(StatusCodes.Status501NotImplemented, "This feature is not yet implemented.");

        return Task.FromResult(result);
    }
}