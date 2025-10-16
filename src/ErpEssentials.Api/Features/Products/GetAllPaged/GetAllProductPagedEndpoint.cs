using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Features.Products.GetAllPaged;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.GetAllPaged;

public class GetAllProductPagedEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<GetAllProductPagedQuery>
    .WithActionResult<PagedResult<ProductResponse>>
{
    private readonly ISender _sender = sender;
    [HttpGet("/api/products", Name = ProductRoutes.GetAllPaged)]
    [ApiExplorerSettings(GroupName = "Inventory / Products")]
    [ProducesResponseType(typeof(PagedResult<ProductResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<PagedResult<ProductResponse>>> HandleAsync([FromQuery] GetAllProductPagedQuery request, CancellationToken cancellationToken = default)
    {

        Result<PagedResult<ProductResponse>> result = await _sender.Send(request, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return Ok(result.Value);
    }
}