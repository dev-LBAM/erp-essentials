using ErpEssentials.Domain.Products;
using FluentValidation;

namespace ErpEssentials.Application.Features.Products.UpdateFinancials;

public class UpdateProductFinancialsCommandValidator : AbstractValidator<UpdateProductFinancialsCommand>
{
    public UpdateProductFinancialsCommandValidator()
    {
        RuleFor(x => x.ProductId).
            NotEmpty()
                .WithErrorCode(ProductErrors.EmptyId.Code)
                .WithMessage(ProductErrors.EmptyId.Message);

        RuleFor(x => x)
            .Must(x => x.NewPrice.HasValue || x.NewCost.HasValue)
            .WithErrorCode(ProductErrors.EmptyFinancialUpdate.Code)
            .WithMessage(ProductErrors.EmptyFinancialUpdate.Message);

        When(x => x.NewPrice.HasValue, () =>
        {
            RuleFor(x => x.NewPrice)
                .GreaterThan(0)
                    .WithErrorCode(ProductErrors.NonPositivePrice.Code)
                    .WithMessage(ProductErrors.NonPositivePrice.Message);
        });

        When(x => x.NewCost.HasValue, () =>
        {
            RuleFor(x => x.NewCost)
                .GreaterThanOrEqualTo(0)
                    .WithErrorCode(ProductErrors.NonNegativeCost.Code)
                    .WithMessage(ProductErrors.NonNegativeCost.Message);
        });
    }
}
