using Ardalis.ApiEndpoints;
using ErpEssentials.Stock.Api.Common;
using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Application.Features.Products.Create;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Stock.Api.Features.Products.Create;

public class CreateProductEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<CreateProductRequest>
    .WithActionResult<ProductResponse>
{
    private readonly ISender _sender = sender;

    [HttpPost("/api/products", Name = ProductRoutes.Create)]
    [ApiExplorerSettings(GroupName = "Inventory / Products")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<ProductResponse>> HandleAsync([FromBody] CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        CreateProductCommand command = new
        (
            request.Sku,
            request.Name,
            request.Description,
            request.Barcode,
            request.Price,
            request.Cost,
            request.BrandId,
            request.CategoryId
        );

        Result<ProductResponse> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return CreatedAtRoute(
            ProductRoutes.GetById,
            new { productId = result.Value.Id },
            result.Value);
    }
}