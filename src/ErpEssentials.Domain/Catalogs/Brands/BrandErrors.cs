using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Domain.Catalogs.Brands;

public static class BrandErrors
{
    public static readonly Error EmptyName = new DomainError("Brand.EmptyName", "The brand name cannot be empty.", ErrorType.Validation);
    public static readonly Error NameInUse = new DomainError("Brand.NameInUse", "This brand name is already in use.", ErrorType.Conflict);
    public static readonly Error InvalidNameFormat = new DomainError("Brand.InvalidNameFormat", "The brand name contains invalid characters.", ErrorType.Validation);
    public static readonly Error NotFound = new DomainError("Brand.NotFound", "The brand with the specified ID was not found.", ErrorType.NotFound);
}