using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Create;

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
) : IRequest<Result<Product>>; 