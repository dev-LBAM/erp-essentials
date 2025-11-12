using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Brands;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Brands.GetById;

public record GetBrandByIdQuery(Guid BrandId) : IRequest<Result<BrandResponse>>;