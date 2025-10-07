using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Lots.RemoveStock;

public record RemoveProductStockCommand(
    Guid ProductId,
    int Quantity
) : IRequest<Result<List<LotResponse>>>;