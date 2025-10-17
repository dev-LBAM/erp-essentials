using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Application.Features.Catalogs.Categories.GetAllPaged;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Catalogs.Categories.GetAllPaged;

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
