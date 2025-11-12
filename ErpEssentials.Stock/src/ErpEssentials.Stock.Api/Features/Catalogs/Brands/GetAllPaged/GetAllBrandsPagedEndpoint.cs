using Ardalis.ApiEndpoints;
using ErpEssentials.Stock.Api.Common;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.Application.Features.Catalogs.Brands.GetAllPaged;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Stock.Api.Features.Catalogs.Brands.GetAllPaged;

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