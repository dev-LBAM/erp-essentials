// Location: src/ErpEssentials.Api/Features/Catalogs/Brands/UpdateName/UpdateBrandNameEndpoint.cs
using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Application.Features.Catalogs.Brands.UpdateName;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Catalogs.Brands.UpdateName;

public class UpdateBrandNameEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;

    [HttpPut("/api/brands/{id:guid}", Name = BrandRoutes.UpdateName)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid id,[FromBody] UpdateBrandNameRequest request,CancellationToken cancellationToken = default)
    {

        UpdateBrandNameCommand command = new(id, request.NewName);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return NoContent();
    }
}
