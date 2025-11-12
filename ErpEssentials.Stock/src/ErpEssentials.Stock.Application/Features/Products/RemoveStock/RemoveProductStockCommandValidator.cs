using ErpEssentials.Stock.Domain.Products.Lots;
using FluentValidation;

namespace ErpEssentials.Stock.Application.Features.Products.RemoveStock;

public class RemoveProductStockCommandValidator : AbstractValidator<RemoveProductStockCommand>
{
    public RemoveProductStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
                .WithErrorCode(LotErrors.MissingProductId.Code)
                .WithMessage(LotErrors.MissingProductId.Message);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
                .WithErrorCode(LotErrors.NonPositiveQuantity.Code)
                .WithMessage(LotErrors.NonPositiveQuantity.Message);
    }
}