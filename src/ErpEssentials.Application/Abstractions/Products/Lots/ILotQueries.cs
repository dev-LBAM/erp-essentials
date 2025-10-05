using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Application.Abstractions.Products.Lots;

public interface ILotQueries
{
    Task<Result<LotResponse>> GetResponseByIdAsync(Guid lotId, CancellationToken cancellationToken = default);
}