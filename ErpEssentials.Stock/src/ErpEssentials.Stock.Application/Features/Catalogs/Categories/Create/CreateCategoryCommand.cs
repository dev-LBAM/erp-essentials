using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Categories.Create;

public record CreateCategoryCommand(string Name) : IRequest<Result<CategoryResponse>>;