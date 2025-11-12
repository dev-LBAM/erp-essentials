using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Activate;

public record ActivateProductCommand(Guid ProductId) : IRequest<Result<ProductResponse>>;