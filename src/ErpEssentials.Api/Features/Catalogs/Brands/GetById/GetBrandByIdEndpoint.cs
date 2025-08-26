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

    [HttpGet("/api/brands/{id:guid}", Name = BrandRoutes.GetById)]
    public override async Task<ActionResult<BrandResponse>> HandleAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        GetBrandByIdQuery query = new(id);

        Result<BrandResponse> result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return Ok(result.Value);
    }
}