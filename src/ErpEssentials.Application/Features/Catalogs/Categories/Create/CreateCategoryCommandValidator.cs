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
            .WithMessage(CategoryErrors.EmptyName.Message);
    }
}