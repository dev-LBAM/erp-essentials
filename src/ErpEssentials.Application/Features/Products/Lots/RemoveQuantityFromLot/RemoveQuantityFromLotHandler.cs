using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Lots;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.Lots.RemoveQuantityFromLot;

public class RemoveQuantityFromLotHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveQuantityFromLotCommand, Result<LotResponse>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<LotResponse>> Handle(RemoveQuantityFromLotCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<LotResponse>.Failure(ProductErrors.NotFound);
        
        Result<Lot> lotResult = product.RemoveQuantityFromLot(request.LotId, request.Quantity);

        if (lotResult.IsFailure) return Result<LotResponse>.Failure(lotResult.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Lot lot = lotResult.Value;

        return Result<LotResponse>.Success(LotResponse.FromEntity(lot));
    }
}