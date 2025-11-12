using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Categories.GetById;

public record GetCategoryByIdQuery(Guid CategoryId) : IRequest<Result<CategoryResponse>>;