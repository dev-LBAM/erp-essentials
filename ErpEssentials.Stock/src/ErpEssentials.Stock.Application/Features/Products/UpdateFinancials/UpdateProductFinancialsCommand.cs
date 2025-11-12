using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.UpdateFinancials;

public record UpdateProductFinancialsCommand(
    Guid ProductId,
    decimal? NewPrice,
    decimal? NewCost
) : IRequest<Result<ProductResponse>>;

