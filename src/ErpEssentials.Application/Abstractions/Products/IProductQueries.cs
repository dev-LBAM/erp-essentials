using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Application.Abstractions.Products;

public interface IProductQueries
{
    Task<Result<ProductResponse>> GetResponseByIdAsync(Guid productId, CancellationToken cancellationToken = default);
}