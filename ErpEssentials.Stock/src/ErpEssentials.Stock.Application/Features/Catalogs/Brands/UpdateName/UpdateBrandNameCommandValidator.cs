using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using FluentValidation;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Brands.UpdateName;

public class UpdateBrandNameCommandValidator : AbstractValidator<UpdateBrandNameCommand>
{
    private readonly IBrandRepository _brandRepository;
    public UpdateBrandNameCommandValidator(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;

        RuleFor(x => x.BrandId)
        .NotEmpty()
        .WithErrorCode(BrandErrors.EmptyId.Code)
        .WithMessage(BrandErrors.EmptyId.Message);

        RuleFor(x => x.NewName)
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
    private async Task<bool> BeUniqueName(string newName, CancellationToken cancellationToken)
    {
        return await _brandRepository.IsNameUniqueAsync(newName, cancellationToken);
    }
}