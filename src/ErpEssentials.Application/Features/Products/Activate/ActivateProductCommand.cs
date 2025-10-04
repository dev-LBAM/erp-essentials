using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Activate;

public record ActivateProductCommand(Guid ProductId) : IRequest<Result<ProductResponse>>;