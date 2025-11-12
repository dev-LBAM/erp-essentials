using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Deactivate;

public record DeactivateProductCommand(Guid ProductId) : IRequest<Result<ProductResponse>>;