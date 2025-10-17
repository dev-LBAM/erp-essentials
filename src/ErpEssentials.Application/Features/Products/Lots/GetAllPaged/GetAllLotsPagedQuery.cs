using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Lots.GetAllPaged;

public record GetAllLotsPagedQuery(int Page = 1, int PageSize = 10, string OrderBy = "CreatedAt", bool Ascending = false) : IRequest<Result<PagedResult<LotResponse>>>;