using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.GetAllPaged;

public record GetAllProductsPagedQuery(int Page = 1, int PageSize = 10, string OrderBy = "Name", bool Ascending = true) : IRequest<Result<PagedResult<ProductResponse>>>;