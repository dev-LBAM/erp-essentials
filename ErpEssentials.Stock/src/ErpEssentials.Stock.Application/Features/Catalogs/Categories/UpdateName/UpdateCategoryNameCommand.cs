using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Categories.UpdateName;

public record UpdateCategoryNameCommand(Guid CategoryId, string NewName) : IRequest<Result<CategoryResponse>>;