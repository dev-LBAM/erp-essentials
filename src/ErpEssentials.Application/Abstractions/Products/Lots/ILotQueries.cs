﻿using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;

namespace ErpEssentials.Application.Abstractions.Products.Lots;

public interface ILotQueries
{
    Task<Result<LotResponse>> GetResponseByIdAsync(Guid lotId, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<LotResponse>>> GetAllPagedAsync(int page = 1, int pageSize = 10, string orderBy = "CreateAt", bool ascending = true, CancellationToken cancellationToken = default);

}