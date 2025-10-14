using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Application.Features.Catalogs.Categories.Create;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Catalogs.Categories.Create;

public class CreateCategoryEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<CreateCategoryRequest>
    .WithActionResult<CategoryResponse>
{
    private readonly ISender _sender = sender;

    [HttpPost("/api/categories", Name = CategoryRoutes.Create)]
    [ApiExplorerSettings(GroupName = "Inventory / Categories")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<CategoryResponse>> HandleAsync([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        CreateCategoryCommand command = new( Name: request.Name );

        Result<CategoryResponse> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return CreatedAtRoute(
            CategoryRoutes.GetById,
            new { categoryId = result.Value.Id },
            result.Value);
    }
}