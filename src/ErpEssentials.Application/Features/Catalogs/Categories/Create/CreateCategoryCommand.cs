using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Categories.Create;

public record CreateCategoryCommand(string Name) : IRequest<Result<CategoryResponse>>;