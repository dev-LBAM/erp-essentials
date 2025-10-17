using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.GetAllPaged;

public record GetAllProductsPagedQuery(int Page = 1, int PageSize = 10, string OrderBy = "Name", bool Ascending = true) : IRequest<Result<PagedResult<ProductResponse>>>;