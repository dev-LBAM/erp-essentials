using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Application.Features.Catalogs.Brands.Create;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Catalogs.Brands.Create;

public class CreateBrandEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<CreateBrandRequest>
    .WithActionResult<Brand>
{
    private readonly ISender _sender = sender;

    [HttpPost("/api/brands")]
    public override async Task<ActionResult<Brand>> HandleAsync([FromBody] CreateBrandRequest request, CancellationToken cancellationToken = default)
    {
        CreateBrandCommand command = new()
        {
            Name = request.Name,
        };

        Result<Brand> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return CreatedAtRoute(
            BrandRoutes.GetById,
            new { id = result.Value.Id },
            result.Value);
    }
}