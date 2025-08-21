using ErpEssentials.Domain.Catalogs.Categories;
using FluentValidation;

namespace ErpEssentials.Application.Features.Catalogs.Categories.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(CategoryErrors.EmptyName.Code)
            .WithMessage(CategoryErrors.EmptyName.Message)
            .Matches("^[a-zA-Z0-9- ]*$")
            .WithErrorCode(CategoryErrors.InvalidNameFormat.Code)
            .WithMessage(CategoryErrors.InvalidNameFormat.Message);
    }
}