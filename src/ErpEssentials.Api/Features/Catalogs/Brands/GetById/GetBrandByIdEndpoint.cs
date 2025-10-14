using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Application.Features.Catalogs.Brands.GetById;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Catalogs.Brands.GetById;

public class GetBrandByIdEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<BrandResponse>
{

    public readonly ISender _sender = sender;

    [HttpGet("/api/brands/{brandId:guid}", Name = BrandRoutes.GetById)]
    [ApiExplorerSettings(GroupName = "Inventory / Brands")]
    [ProducesResponseType(typeof(BrandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public override async Task<ActionResult<BrandResponse>> HandleAsync([FromRoute] Guid brandId, CancellationToken cancellationToken = default)
    {
        GetBrandByIdQuery query = new(brandId);

        Result<BrandResponse> result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return Ok(result.Value);
    }
}