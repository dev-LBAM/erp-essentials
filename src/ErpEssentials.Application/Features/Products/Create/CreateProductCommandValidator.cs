using ErpEssentials.Domain.Products;
using FluentValidation;

namespace ErpEssentials.Application.Features.Products.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty()
            .WithErrorCode(ProductErrors.EmptySku.Code)
            .WithMessage(ProductErrors.EmptySku.Message);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(ProductErrors.EmptyName.Code)
            .WithMessage(ProductErrors.EmptyName.Message)
            .Matches("^[a-zA-Z0-9- ]*$")
            .WithErrorCode(ProductErrors.InvalidNameFormat.Code)
            .WithMessage(ProductErrors.InvalidNameFormat.Message);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithErrorCode(ProductErrors.NonPositivePrice.Code)
            .WithMessage(ProductErrors.NonPositivePrice.Message);

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(ProductErrors.NonNegativeCost.Code)
            .WithMessage(ProductErrors.NonNegativeCost.Message);

        RuleFor(x => x.BrandId)
            .NotEmpty()
            .WithErrorCode(ProductErrors.EmptyBrandId.Code)
            .WithMessage(ProductErrors.EmptyBrandId.Message);

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithErrorCode(ProductErrors.EmptyCategoryId.Code)
            .WithMessage(ProductErrors.EmptyCategoryId.Message);
    }
}