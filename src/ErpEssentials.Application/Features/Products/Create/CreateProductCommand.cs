using ErpEssentials.Domain.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Create;

public class CreateProductCommand : IRequest<Result<Product>>
{
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Barcode { get; init; }
    public decimal Price { get; init; }
    public decimal Cost { get; init; }
    public Guid BrandId { get; init; }
    public Guid CategoryId { get; init; }
}