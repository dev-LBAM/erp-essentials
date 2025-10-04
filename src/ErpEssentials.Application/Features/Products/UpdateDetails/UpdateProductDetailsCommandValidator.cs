using ErpEssentials.Domain.Products;
using FluentValidation;

namespace ErpEssentials.Application.Features.Products.UpdateDetails;

public class UpdateProductDetailsCommandValidator : AbstractValidator<UpdateProductDetailsCommand>
{
    private readonly IProductRepository _productRepository;
    public UpdateProductDetailsCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(x => x.ProductId).
            NotEmpty()
                .WithErrorCode(ProductErrors.EmptyId.Code)
                .WithMessage(ProductErrors.EmptyId.Message);

        RuleFor(x => x)
            .Must(x => x.NewName != null
                || !string.IsNullOrWhiteSpace(x.NewBarcode)
                || !string.IsNullOrWhiteSpace(x.NewDescription))
            .WithErrorCode(ProductErrors.EmptyDetailUpdate.Code)
            .WithMessage(ProductErrors.EmptyDetailUpdate.Message);

        When(x => x.NewName is not null, () =>
        {
            RuleFor(x => x.NewName)
                .NotEmpty()
                    .WithErrorCode(ProductErrors.EmptyName.Code)
                    .WithMessage(ProductErrors.EmptyName.Message)
                .MaximumLength(100)
                    .WithErrorCode(ProductErrors.NameTooLong.Code)
                    .WithMessage(ProductErrors.NameTooLong.Message)
                .Matches("^[a-zA-Z0-9À-ÿ\\s\\-/()']*$")
                    .WithErrorCode(ProductErrors.InvalidNameFormat.Code)
                    .WithMessage(ProductErrors.InvalidNameFormat.Message);
        });

        When(x => !string.IsNullOrWhiteSpace(x.NewBarcode), () =>
        {
            RuleFor(x => x.NewBarcode)
                .MaximumLength(13)
                    .WithErrorCode(ProductErrors.BarcodeTooLong.Code)
                    .WithMessage(ProductErrors.BarcodeTooLong.Message)
                .MustAsync(BeUniqueBarcode)
                    .WithErrorCode(ProductErrors.BarcodeInUse.Code)
                    .WithMessage(ProductErrors.BarcodeInUse.Message);
        });

        When(x => !string.IsNullOrWhiteSpace(x.NewDescription), () =>
        {
            RuleFor(x => x.NewDescription)
                .MaximumLength(500)
                .WithErrorCode(ProductErrors.DescriptionTooLong.Code)
                .WithMessage(ProductErrors.DescriptionTooLong.Message);
        });
    }

    private async Task<bool> BeUniqueBarcode(string barcode, CancellationToken cancellationToken)
    {
        return await _productRepository.IsBarcodeUniqueAsync(barcode, cancellationToken);
    }
}