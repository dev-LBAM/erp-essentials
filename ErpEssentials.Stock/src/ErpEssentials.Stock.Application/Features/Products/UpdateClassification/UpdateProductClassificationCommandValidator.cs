using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.Domain.Products;
using FluentValidation;

namespace ErpEssentials.Stock.Application.Features.Products.UpdateClassification;

public class UpdateProductClassificationCommandValidator : AbstractValidator<UpdateProductClassificationCommand>
{
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductClassificationCommandValidator(IBrandRepository brandRepository, ICategoryRepository categoryRepository)
    {
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;

        RuleFor(x => x.ProductId).
            NotEmpty()
                .WithErrorCode(ProductErrors.EmptyId.Code)
                .WithMessage(ProductErrors.EmptyId.Message);

        RuleFor(x => x)
            .Must(x => x.NewBrandId.HasValue || x.NewCategoryId.HasValue)
                .WithErrorCode(ProductErrors.EmptyClassificationUpdate.Code)
                .WithMessage(ProductErrors.EmptyClassificationUpdate.Message);

        When(x => x.NewBrandId.HasValue && x.NewBrandId != Guid.Empty, () =>
        {
            RuleFor(x => x.NewBrandId)
                .NotEmpty()
                    .WithErrorCode(ProductErrors.EmptyBrandId.Code)
                    .WithMessage(ProductErrors.EmptyBrandId.Message)
                .MustAsync(BrandMustExist)
                    .WithErrorCode(BrandErrors.NotFound.Code)
                    .WithMessage(BrandErrors.NotFound.Message);
        });

        When(x => x.NewCategoryId.HasValue && x.NewCategoryId != Guid.Empty, () =>
        {
            RuleFor(x => x.NewCategoryId)
                .NotEmpty()
                    .WithErrorCode(ProductErrors.EmptyCategoryId.Code)
                    .WithMessage(ProductErrors.EmptyCategoryId.Message)
                .MustAsync(CategoryMustExist)
                    .WithErrorCode(CategoryErrors.NotFound.Code)
                    .WithMessage(CategoryErrors.NotFound.Message);
        });

    }

    private async Task<bool> BrandMustExist(Guid? newBrandId, CancellationToken cancellationToken)
    {
        Brand? brand = await _brandRepository.GetByIdAsync(newBrandId ?? Guid.Empty, cancellationToken);
        return brand is not null;
    }

    private async Task<bool> CategoryMustExist(Guid? newCategoryId, CancellationToken cancellationToken)
    {
        Category? category = await _categoryRepository.GetByIdAsync(newCategoryId ?? Guid.Empty, cancellationToken);
        return category is not null;
    }
}


