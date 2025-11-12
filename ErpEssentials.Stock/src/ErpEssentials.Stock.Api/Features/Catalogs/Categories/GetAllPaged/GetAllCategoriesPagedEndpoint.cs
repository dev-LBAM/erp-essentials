using Ardalis.ApiEndpoints;
using ErpEssentials.Stock.Api.Common;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Stock.Application.Features.Catalogs.Categories.GetAllPaged;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Stock.Api.Features.Catalogs.Categories.GetAllPaged;

public class GetAllCategoriesPagedEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;
    [HttpGet("/api/categories", Name = CategoryRoutes.GetAllPaged)]
    [ApiExplorerSettings(GroupName = "Inventory / Categories")]
    [ProducesResponseType(typeof(PagedResult<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync([FromQuery] GetAllCategoriesPagedQuery request, CancellationToken cancellationToken = default)
    {
        Result<PagedResult<CategoryResponse>> result = await _sender.Send(request, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return Ok(result.Value);
    }
}
