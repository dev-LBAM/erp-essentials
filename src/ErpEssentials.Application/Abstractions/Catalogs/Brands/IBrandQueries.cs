using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Application.Abstractions.Catalogs.Brands;

public interface IBrandQueries
{
    Task<Result<BrandResponse>> GetResponseByIdAsync(Guid brandId, CancellationToken cancellationToken = default);
}