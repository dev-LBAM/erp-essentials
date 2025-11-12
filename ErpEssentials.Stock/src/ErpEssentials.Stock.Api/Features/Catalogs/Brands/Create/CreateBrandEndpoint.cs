using Ardalis.ApiEndpoints;
using ErpEssentials.Stock.Api.Common;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.Application.Features.Catalogs.Brands.Create;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Stock.Api.Features.Catalogs.Brands.Create;

public class CreateBrandEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<CreateBrandRequest>
    .WithActionResult<Brand>
{
    private readonly ISender _sender = sender;

    [HttpPost("/api/brands", Name = BrandRoutes.Create)]
    [ApiExplorerSettings(GroupName = "Inventory / Brands")]
    [ProducesResponseType(typeof(BrandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<Brand>> HandleAsync([FromBody] CreateBrandRequest request, CancellationToken cancellationToken = default)
    {
        CreateBrandCommand command = new( Name: request.Name );

        Result<BrandResponse> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return CreatedAtRoute(
            BrandRoutes.GetById,
            new { brandId = result.Value.Id },
            result.Value);
    }
}