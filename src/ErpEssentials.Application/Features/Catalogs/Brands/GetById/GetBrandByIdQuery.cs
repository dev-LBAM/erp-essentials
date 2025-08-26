using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using ErpEssentials.Application.Contracts.Catalogs.Brands;

namespace ErpEssentials.Application.Features.Catalogs.Brands.GetById;

public record GetBrandByIdQuery(Guid BrandId) : IRequest<Result<BrandResponse>>;