using ErpEssentials.Application.Contracts.Products;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.UpdateClassification;

public record UpdateProductClassificationCommand
(
    Guid ProductId,
    Guid? NewBrandId,
    Guid? NewCategoryId
): IRequest<Result<ProductResponse>>;