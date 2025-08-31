using ErpEssentials.Domain.Catalogs.Categories;
using FluentValidation;

namespace ErpEssentials.Application.Features.Catalogs.Categories.UpdateName;

public class UpdateCategoryNameCommandValidator : AbstractValidator<UpdateCategoryNameCommand>
{
    public UpdateCategoryNameCommandValidator()
    {
        RuleFor(x => x.CategoryId)
        .NotEmpty()
        .WithErrorCode(CategoryErrors.EmptyId.Code)
        .WithMessage(CategoryErrors.EmptyId.Message);

        RuleFor(x => x.NewName)
        .NotEmpty()
        .WithErrorCode(CategoryErrors.EmptyName.Code)
        .WithMessage(CategoryErrors.EmptyName.Message)
        .Matches("^[a-zA-Z0-9- ]*$")
        .WithErrorCode(CategoryErrors.InvalidNameFormat.Code)
        .WithMessage(CategoryErrors.InvalidNameFormat.Message);
    }
}