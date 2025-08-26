using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.GetById;

public record GetProductByIdQuery(Guid ProductId) : IRequest<Result<ProductResponse>>;