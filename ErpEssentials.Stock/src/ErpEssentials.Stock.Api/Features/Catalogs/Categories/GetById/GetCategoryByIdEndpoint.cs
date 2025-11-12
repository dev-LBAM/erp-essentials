using Ardalis.ApiEndpoints;
using ErpEssentials.Stock.Api.Common;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Stock.Application.Features.Catalogs.Categories.GetById;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Stock.Api.Features.Catalogs.Categories.GetById;

public class GetCategoryByIdEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<CategoryResponse>
{
    private readonly ISender _sender = sender;

    [HttpGet("/api/categories/{categoryId:guid}", Name = CategoryRoutes.GetById)]
    [ApiExplorerSettings(GroupName = "Inventory / Categories")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public override async Task<ActionResult<CategoryResponse>> HandleAsync([FromRoute]Guid categoryId, CancellationToken cancellationToken = default)
    {
        GetCategoryByIdQuery query = new(categoryId);

        Result<CategoryResponse> result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return Ok(result.Value);
    }
}