using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.Domain.Products;
using FluentValidation;

namespace ErpEssentials.Application.Features.Products.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;
    public CreateProductCommandValidator(IProductRepository productRepository, IBrandRepository brandRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Sku)
            .NotEmpty()
                .WithErrorCode(ProductErrors.EmptySku.Code)
                .WithMessage(ProductErrors.EmptySku.Message)
            .MustAsync(BeUniqueSku)
                .WithErrorCode(ProductErrors.SkuConflict.Code)
                .WithMessage(ProductErrors.SkuConflict.Message);

        RuleFor(x => x.Name)
            .NotEmpty()
                .WithErrorCode(ProductErrors.EmptyName.Code)
                .WithMessage(ProductErrors.EmptyName.Message)
            .Matches("^[a-zA-Z0-9À-ÿ\\s\\-/()']+$")
                .WithErrorCode(ProductErrors.InvalidNameFormat.Code)
                .WithMessage(ProductErrors.InvalidNameFormat.Message)
            .MaximumLength(100)
                .WithErrorCode(ProductErrors.NameTooLong.Code)
                .WithMessage(ProductErrors.NameTooLong.Message);

        When(x => !string.IsNullOrWhiteSpace(x.Barcode), () =>
        {
            RuleFor(x => x.Barcode)
                .MaximumLength(13)
                    .WithErrorCode(ProductErrors.BarcodeTooLong.Code)
                    .WithMessage(ProductErrors.BarcodeTooLong.Message)
                .MustAsync(BeUniqueBarcode)
                    .WithErrorCode(ProductErrors.BarcodeInUse.Code)
                    .WithMessage(ProductErrors.BarcodeInUse.Message);
        });

        When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(500)
                    .WithErrorCode(ProductErrors.DescriptionTooLong.Code)
                    .WithMessage(ProductErrors.DescriptionTooLong.Message);
        });

        RuleFor(x => x.Price)
            .GreaterThan(0)
                .WithErrorCode(ProductErrors.NonPositivePrice.Code)
                .WithMessage(ProductErrors.NonPositivePrice.Message);

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0)
                .WithErrorCode(ProductErrors.NonNegativeCost.Code)
                .WithMessage(ProductErrors.NonNegativeCost.Message);

        RuleFor(x => x.BrandId)
            .NotEmpty()
                .WithErrorCode(ProductErrors.EmptyBrandId.Code)
                .WithMessage(ProductErrors.EmptyBrandId.Message)
            .MustAsync(BrandMustExist)
                .WithErrorCode(BrandErrors.NotFound.Code)
                .WithMessage(BrandErrors.NotFound.Message);

        RuleFor(x => x.CategoryId)
            .NotEmpty()
                .WithErrorCode(ProductErrors.EmptyCategoryId.Code)
                .WithMessage(ProductErrors.EmptyCategoryId.Message)
            .MustAsync(CategoryMustExist)
                .WithErrorCode(CategoryErrors.NotFound.Code)
                .WithMessage(CategoryErrors.NotFound.Message);
    }

    private async Task<bool> BeUniqueBarcode(string barcode, CancellationToken cancellationToken)
    {
        return await _productRepository.IsBarcodeUniqueAsync(barcode, cancellationToken);
    }

    private async Task<bool> BeUniqueSku(string sku, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetBySkuAsync(sku, cancellationToken);
        return product is null;
    }

    private async Task<bool> BrandMustExist(Guid brandId, CancellationToken cancellationToken)
    {
        Brand? brand = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
        return brand is not null;
    }

    private async Task<bool> CategoryMustExist(Guid categoryId, CancellationToken cancellationToken)
    {
        Category? category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        return category is not null;
    }
}