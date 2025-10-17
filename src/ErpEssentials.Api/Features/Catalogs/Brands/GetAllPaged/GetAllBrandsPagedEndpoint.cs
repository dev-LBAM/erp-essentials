using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Application.Features.Catalogs.Brands.GetAllPaged;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Catalogs.Brands.GetAllPaged;

public class GetAllBrandsPagedEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;
    [HttpGet("/api/brands", Name = BrandRoutes.GetAllPaged)]
    [ApiExplorerSettings(GroupName = "Catalogs / Brands")]
    [ProducesResponseType(typeof(PagedResult<BrandResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync([FromQuery] GetAllBrandsPagedQuery request, CancellationToken cancellationToken = default)
    {
        Result<PagedResult<BrandResponse>> result = await _sender.Send(request, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return Ok(result.Value);
    }
}