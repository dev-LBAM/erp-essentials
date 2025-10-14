﻿using Ardalis.ApiEndpoints;
using ErpEssentials.Api.Common;
using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Application.Features.Products.Activate;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Features.Products.Activate;

public class ActivateProductEndpoint(ISender sender) : EndpointBase
{
    private readonly ISender _sender = sender;
    [HttpPatch("/api/products/{productId:guid}/activate", Name = ProductRoutes.Activate)]
    [ApiExplorerSettings(GroupName = "Inventory / Products")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> HandleAsync([FromRoute] Guid productId, CancellationToken cancellationToken = default)
    {
        ActivateProductCommand command = new(productId);
        Result<ProductResponse> result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return this.HandleFailure(result.Error);
        }
        return Ok(result.Value);
    }
}