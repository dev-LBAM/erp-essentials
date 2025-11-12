using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.GetById;

public record GetProductByIdQuery(Guid ProductId) : IRequest<Result<ProductResponse>>;