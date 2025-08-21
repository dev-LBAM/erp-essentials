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
            .WithMessage(BrandErrors.EmptyName.Message)
            .Matches("^[a-zA-Z0-9- ]*$")
            .WithErrorCode(BrandErrors.InvalidNameFormat.Code)
            .WithMessage(BrandErrors.InvalidNameFormat.Message);
    }
}