using ErpEssentials.Domain.Catalogs.Categories;
using FluentValidation;

namespace ErpEssentials.Application.Features.Catalogs.Categories.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Name)
            .NotEmpty()
                .WithErrorCode(CategoryErrors.EmptyName.Code)
                .WithMessage(CategoryErrors.EmptyName.Message)
            .MaximumLength(150)
                .WithErrorCode(CategoryErrors.NameTooLong.Code)
                .WithMessage(CategoryErrors.NameTooLong.Message)
            .Matches("^[a-zA-Z0-9À-ÿ\\s\\-/()']+$")
                .WithErrorCode(CategoryErrors.InvalidNameFormat.Code)
                .WithMessage(CategoryErrors.InvalidNameFormat.Message)
            .MustAsync(BeUniqueName)
                .WithErrorCode(CategoryErrors.NameInUse.Code)
                .WithMessage(CategoryErrors.NameInUse.Message);
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _categoryRepository.IsNameUniqueAsync(name, cancellationToken);
    }
}