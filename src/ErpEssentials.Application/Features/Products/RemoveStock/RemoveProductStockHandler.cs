using ErpEssentials.Application.Contracts.Products.Lots;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Lots;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Products.RemoveStock;

public class RemoveProductStockHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveProductStockCommand, Result<List<LotResponse>>>
{

    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<List<LotResponse>>> Handle(RemoveProductStockCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<List<LotResponse>>.Failure(ProductErrors.NotFound);


        Result<List<Lot>> removeStockResult = product.RemoveStock(request.Quantity);

        if (removeStockResult.IsFailure) return Result<List<LotResponse>>.Failure(removeStockResult.Error);

        List<Lot> newStock = removeStockResult.Value;


        await _unitOfWork.SaveChangesAsync(cancellationToken);

        List<LotResponse> response = [.. removeStockResult.Value.Select(LotResponse.FromEntity)];

        return Result<List<LotResponse>>.Success(response);
    }
}