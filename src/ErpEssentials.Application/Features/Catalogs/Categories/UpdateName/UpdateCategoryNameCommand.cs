using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Categories.UpdateName;

public record UpdateCategoryNameCommand(Guid CategoryId, string NewName) : IRequest<Result>;