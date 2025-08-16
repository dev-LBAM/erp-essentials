using ErpEssentials.Domain.Catalogs.Brands; 
using FluentValidation;

namespace ErpEssentials.Application.Features.Catalogs.Brands.Create;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(BrandErrors.EmptyName.Code)
            .WithMessage(BrandErrors.EmptyName.Message);
    }
}