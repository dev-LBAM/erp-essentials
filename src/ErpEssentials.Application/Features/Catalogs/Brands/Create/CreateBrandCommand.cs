using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Brands.Create;

public record CreateBrandCommand(string Name) : IRequest<Result<BrandResponse>>;