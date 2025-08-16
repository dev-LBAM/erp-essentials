using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Brands.Create;

public class CreateBrandCommand : IRequest<Result<Brand>>
{
    public string Name { get; init; } = string.Empty;
}