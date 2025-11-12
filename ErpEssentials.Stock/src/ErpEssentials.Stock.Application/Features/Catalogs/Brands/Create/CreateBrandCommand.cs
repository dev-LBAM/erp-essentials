using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Brands.Create;

public record CreateBrandCommand(string Name) : IRequest<Result<BrandResponse>>;