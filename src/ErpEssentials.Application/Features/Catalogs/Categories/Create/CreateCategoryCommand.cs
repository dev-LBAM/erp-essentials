using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Categories.Create;

public class CreateCategoryCommand : IRequest<Result<Category>>
{
    public string Name { get; init; } = string.Empty;
}