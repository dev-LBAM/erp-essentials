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

    [HttpPatch("/api/brands/{brandId:guid}", Name = BrandRoutes.UpdateName)]
    [ApiExplorerSettings(GroupName = "Inventory / Brands")]
    [ProducesResponseType(typeof(BrandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid brandId, [FromBody] UpdateBrandNameRequest request,CancellationToken cancellationToken = default)
    {

        UpdateBrandNameCommand command = new(brandId, request.NewName);

        Result<BrandResponse> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return Ok(result.Value);
    }
}
