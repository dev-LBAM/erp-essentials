using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Deactivate;

public record DeactivateProductCommand(Guid ProductId) : IRequest<Result<ProductResponse>>;