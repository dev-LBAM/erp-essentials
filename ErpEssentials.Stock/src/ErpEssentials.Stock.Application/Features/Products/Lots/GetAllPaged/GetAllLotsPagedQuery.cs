using ErpEssentials.Stock.Application.Contracts.Products.Lots;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.Lots.GetAllPaged;

public record GetAllLotsPagedQuery(int Page = 1, int PageSize = 10, string OrderBy = "CreatedAt", bool Ascending = false) : IRequest<Result<PagedResult<LotResponse>>>;