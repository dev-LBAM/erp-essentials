using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Api.Contracts.Catalogs.Categories.Create;
using ErpEssentials.Application.Features.Catalogs.Categories.Create;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Catalogs.Categories.Create;

public class CreateCategoryEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<CreateCategoryRequest>
    .WithActionResult<Category>
{
    private readonly ISender _sender = sender;

    [HttpPost("/api/categories")]
    public override async Task<ActionResult<Category>> HandleAsync([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        CreateCategoryCommand command = new()
        {
            Name = request.Name,
        };

        Result<Category> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return CreatedAtRoute(
            CategoryRoutes.GetById,
            new { id = result.Value.Id },
            result.Value);
    }
}