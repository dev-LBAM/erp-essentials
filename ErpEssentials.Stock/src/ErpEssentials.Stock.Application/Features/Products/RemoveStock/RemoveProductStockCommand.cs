using ErpEssentials.Stock.Application.Contracts.Products.Lots;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.RemoveStock;

public record RemoveProductStockCommand(
    Guid ProductId,
    int Quantity
) : IRequest<Result<List<LotResponse>>>;