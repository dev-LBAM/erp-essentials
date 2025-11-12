using ErpEssentials.Stock.Domain.Products;
using FluentValidation;

namespace ErpEssentials.Stock.Application.Features.Products.Deactivate;

public class DeactivateProductCommandValidator : AbstractValidator<DeactivateProductCommand>
{
    public DeactivateProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
                .WithErrorCode(ProductErrors.EmptyId.Code)
                .WithMessage(ProductErrors.EmptyId.Message);
    }
}