using ErpEssentials.Domain.Products.Lots;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpEssentials.Application.Features.Products.Lots.ReceiveStock;

public class ReceiveProductStockCommandValidator : AbstractValidator<ReceiveProductStockCommand>
{
    public ReceiveProductStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
                .WithErrorCode(LotErrors.MissingProductId.Code)
                .WithMessage(LotErrors.MissingProductId.Message);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
                .WithErrorCode(LotErrors.NonPositiveQuantity.Code)
                .WithMessage(LotErrors.NonPositiveQuantity.Message);

        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0)
                .WithErrorCode(LotErrors.NonNegativePurchasePrice.Code)
                .WithMessage(LotErrors.NonNegativePurchasePrice.Message);
    }
}
