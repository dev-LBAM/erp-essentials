using ErpEssentials.Application.Abstractions.Products.Lots;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Data;
using ErpEssentials.Domain.Products.Lots;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpEssentials.Application.Features.Products.Lots.AddQuantityToLot;

public class AddQuantityToLotHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddQuantityToLotCommand, Result<LotResponse>>
{
    IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<LotResponse>> Handle(AddQuantityToLotCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<LotResponse>.Failure(ProductErrors.NotFound);

        Result<Lot> addQuantityToLotResult = product.AddQuantityToLot(request.LotId, request.Quantity);

        if (addQuantityToLotResult.IsFailure) return Result<LotResponse>.Failure(addQuantityToLotResult.Error);

        Lot lot = addQuantityToLotResult.Value;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<LotResponse>.Success(LotResponse.FromEntity(lot));
    }
}
