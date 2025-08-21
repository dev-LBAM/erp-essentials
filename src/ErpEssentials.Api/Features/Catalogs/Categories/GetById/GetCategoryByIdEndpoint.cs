using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Catalogs.Categories.GetById;

public class GetCategoryByIdEndpoint : EndpointBaseAsync
    .WithRequest<Guid>
    .WithoutResult
{
    [HttpGet("/api/categories/{id:guid}", Name = CategoryRoutes.GetById)]
    public override Task<ActionResult> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // TODO: Implement the real logic by creating and sending a GetProductByIdQuery.

        ActionResult result = StatusCode(StatusCodes.Status501NotImplemented, "This feature is not yet implemented.");

        return Task.FromResult(result);
    }
}