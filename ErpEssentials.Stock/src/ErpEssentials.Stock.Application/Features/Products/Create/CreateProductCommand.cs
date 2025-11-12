using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Create;

public record CreateProductCommand
(
    string Sku,
    string Name,
    string? Description,
    string? Barcode,
    decimal Price,
    decimal Cost,
    Guid BrandId,
    Guid CategoryId
) : IRequest<Result<ProductResponse>>; 