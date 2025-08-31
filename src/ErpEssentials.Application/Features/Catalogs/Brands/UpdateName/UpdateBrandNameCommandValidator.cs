using ErpEssentials.Domain.Catalogs.Brands;
using FluentValidation;

namespace ErpEssentials.Application.Features.Catalogs.Brands.UpdateName;

public class UpdateBrandNameCommandValidator : AbstractValidator<UpdateBrandNameCommand>
{
    public UpdateBrandNameCommandValidator()
    {
        RuleFor(x => x.BrandId)
        .NotEmpty()
        .WithErrorCode(BrandErrors.EmptyId.Code)
        .WithMessage(BrandErrors.EmptyId.Message);

        RuleFor(x => x.NewName)
            .NotEmpty()
            .WithErrorCode(BrandErrors.EmptyName.Code)
            .WithMessage(BrandErrors.EmptyName.Message)
            .Matches("^[a-zA-Z0-9- ]*$")
            .WithErrorCode(BrandErrors.InvalidNameFormat.Code)
            .WithMessage(BrandErrors.InvalidNameFormat.Message);
    }
}