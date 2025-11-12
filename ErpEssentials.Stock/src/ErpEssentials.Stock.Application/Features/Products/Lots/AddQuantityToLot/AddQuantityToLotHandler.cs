using ErpEssentials.Stock.Application.Abstractions.Products.Lots;
using ErpEssentials.Stock.Application.Contracts.Products.Lots;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.Domain.Products.Data;
using ErpEssentials.Stock.Domain.Products.Lots;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpEssentials.Stock.Application.Features.Products.Lots.AddQuantityToLot;

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
