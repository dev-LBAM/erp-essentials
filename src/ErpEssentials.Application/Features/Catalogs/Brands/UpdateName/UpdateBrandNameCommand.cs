using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Brands.UpdateName;

public record UpdateBrandNameCommand(Guid BrandId, string NewName) : IRequest<Result<BrandResponse>>;
