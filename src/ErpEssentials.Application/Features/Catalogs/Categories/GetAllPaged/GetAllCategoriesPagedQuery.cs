using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Categories.GetAllPaged;

public record GetAllCategoriesPagedQuery(int Page = 1, int PageSize = 10, string OrderBy = "Name", bool Ascending = true) : IRequest<Result<PagedResult<CategoryResponse>>>;