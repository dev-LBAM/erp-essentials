using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Api.Contracts.Products.Create;
using ErpEssentials.Application.Features.Products.Create;
using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.Create;

public class CreateProductEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<CreateProductRequest>
    .WithActionResult<Product>
{
    private readonly ISender _sender = sender;

    [HttpPost("/api/products")]
    public override async Task<ActionResult<Product>> HandleAsync([FromBody] CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        Console.Write("hi");
        CreateProductCommand command = new()
        {
            Sku = request.Sku,
            Name = request.Name,
            Description = request.Description,
            Barcode = request.Barcode,
            Price = request.Price,
            Cost = request.Cost,
            BrandId = request.BrandId,
            CategoryId = request.CategoryId
        };

        Result<Product> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }

        return CreatedAtRoute(
            ProductRoutes.GetById,
            new { id = result.Value.Id },
            result.Value);
    }
}