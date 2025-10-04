using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Application.Features.Catalogs.Categories.UpdateName;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Catalogs.Categories.UpdateName;

public class UpdateCategoryNameEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;
    [HttpPatch("/api/categories/{id:guid}", Name = CategoryRoutes.UpdateName)]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryResponse>> HandleAsync([FromRoute] Guid id, [FromBody] UpdateCategoryNameRequest request, CancellationToken cancellationToken = default)
    {
        UpdateCategoryNameCommand command = new(id, request.NewName);

        Result<CategoryResponse> result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return Ok(result.Value);
    }
}
