using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.UpdateDetails;

public record UpdateProductDetailsCommand(
    Guid ProductId,
    string? NewName,
    string? NewDescription,
    string? NewBarcode
): IRequest<Result<ProductResponse>>;