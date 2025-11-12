using ErpEssentials.Stock.Application.Contracts.Products;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Products.UpdateClassification;

public record UpdateProductClassificationCommand
(
    Guid ProductId,
    Guid? NewBrandId,
    Guid? NewCategoryId
): IRequest<Result<ProductResponse>>;