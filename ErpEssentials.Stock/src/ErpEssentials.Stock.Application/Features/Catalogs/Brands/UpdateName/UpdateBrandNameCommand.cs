using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Brands.UpdateName;

public record UpdateBrandNameCommand(Guid BrandId, string NewName) : IRequest<Result<BrandResponse>>;
