using ErpEssentials.Application.Abstractions.Products.Lots;
using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Data;
using ErpEssentials.Domain.Products.Lots;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.ReceiveStock;

public class ReceiveProductStockHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ILotRepository lotRepository,
    ILotQueries lotQueries) : IRequestHandler<ReceiveProductStockCommand, Result<LotResponse>>
{

    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILotRepository _lotRepository = lotRepository;
    private readonly ILotQueries _lotQueries = lotQueries;
    public async Task<Result<LotResponse>> Handle(ReceiveProductStockCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<LotResponse>.Failure(ProductErrors.NotFound);
        
        CreateLotData createLotData = new
        (
            request.Quantity,
            request.PurchasePrice,
            request.ExpirationDate
        );

        Result<Lot> receiveStockResult = product.ReceiveStock(createLotData);

        if (receiveStockResult.IsFailure) return Result<LotResponse>.Failure(receiveStockResult.Error);

        Lot newStock = receiveStockResult.Value;

        await _lotRepository.AddAsync(newStock, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _lotQueries.GetResponseByIdAsync(newStock.Id, cancellationToken);
    }
}