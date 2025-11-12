using ErpEssentials.Stock.Domain.Products;
using FluentValidation;

namespace ErpEssentials.Stock.Application.Features.Products.Activate;

public class ActivateProductCommandValidator : AbstractValidator<ActivateProductCommand>
{
    public ActivateProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
                .WithErrorCode(ProductErrors.EmptyId.Code)
                .WithMessage(ProductErrors.EmptyId.Message);
    }
}