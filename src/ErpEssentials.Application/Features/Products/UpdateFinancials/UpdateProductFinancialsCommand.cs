using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.UpdateFinancials;

public record UpdateProductFinancialsCommand(
    Guid ProductId,
    decimal? NewPrice,
    decimal? NewCost
) : IRequest<Result<ProductResponse>>;

