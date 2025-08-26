using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using ErpEssentials.Application.Contracts.Catalogs.Categories;

namespace ErpEssentials.Application.Features.Catalogs.Categories.GetById;

public record GetCategoryByIdQuery(Guid CategoryId) : IRequest<Result<CategoryResponse>>;