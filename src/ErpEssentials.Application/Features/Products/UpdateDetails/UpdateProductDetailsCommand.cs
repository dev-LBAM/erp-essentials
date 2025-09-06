using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.UpdateDetails;

public record UpdateProductDetailsCommand(
    Guid ProductId,
    string? NewName,
    string? NewDescription,
    string? NewBarcode
): IRequest<Result<ProductResponse>>;