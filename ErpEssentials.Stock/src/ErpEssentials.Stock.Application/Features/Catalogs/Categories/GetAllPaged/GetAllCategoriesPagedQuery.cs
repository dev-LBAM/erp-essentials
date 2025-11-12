using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Stock.SharedKernel.Pagination;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Categories.GetAllPaged;

public record GetAllCategoriesPagedQuery(int Page = 1, int PageSize = 10, string OrderBy = "Name", bool Ascending = true) : IRequest<Result<PagedResult<CategoryResponse>>>;