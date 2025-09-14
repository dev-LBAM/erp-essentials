using ErpEssentials.Domain.Catalogs.Brands; 
using FluentValidation;

namespace ErpEssentials.Application.Features.Catalogs.Brands.Create;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    private readonly IBrandRepository _brandRepository;
    public CreateBrandCommandValidator(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;

        RuleFor(x => x.Name)
            .NotEmpty()
                .WithErrorCode(BrandErrors.EmptyName.Code)
                .WithMessage(BrandErrors.EmptyName.Message)
            .MaximumLength(100)
                .WithErrorCode(BrandErrors.NameTooLong.Code)
                .WithMessage(BrandErrors.NameTooLong.Message)
            .Matches("^[a-zA-ZÀ-ÿ\\s'-]+$")
                .WithErrorCode(BrandErrors.InvalidNameFormat.Code)
                .WithMessage(BrandErrors.InvalidNameFormat.Message)
            .MustAsync(BeUniqueName)
                .WithErrorCode(BrandErrors.NameInUse.Code)
                .WithMessage(BrandErrors.NameInUse.Message);
    }
    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _brandRepository.IsNameUniqueAsync(name, cancellationToken);
    }
}