using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Lots;
using FluentValidation;

namespace ErpEssentials.Application.Features.Products.Lots.AddQuantityToLot;

public class AddQuantityToLotCommandValidator : AbstractValidator<AddQuantityToLotCommand>
{
    public AddQuantityToLotCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
                .WithErrorCode(ProductErrors.NotFound.Code)
                .WithMessage(ProductErrors.NotFound.Message);

        RuleFor(x => x.LotId)
            .NotEmpty()
                .WithErrorCode(LotErrors.NotFound.Code)
                .WithMessage(LotErrors.NotFound.Message);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
                .WithErrorCode(LotErrors.NonPositiveQuantity.Code)
                .WithMessage(LotErrors.NonPositiveQuantity.Message);
    }
}