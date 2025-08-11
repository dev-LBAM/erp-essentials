using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Catalog.Brands;

public static class BrandErrors
{
    // Brand Errors
    public static readonly Error EmptyBrandName = new DomainError(
        "Brand.EmptyName", "The brand name cannot be empty.", ErrorType.Validation);

    public static readonly Error BrandNameInUse = new DomainError(
        "Brand.NameInUse", "This brand name is already in use.", ErrorType.Conflict);
}