using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Categories.UpdateName;

public record UpdateCategoryNameCommand(Guid CategoryId, string NewName) : IRequest<Result<CategoryResponse>>;