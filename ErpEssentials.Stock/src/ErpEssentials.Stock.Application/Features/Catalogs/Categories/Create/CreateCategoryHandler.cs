using ErpEssentials.Stock.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Stock.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using ErpEssentials.Stock.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Stock.Application.Features.Catalogs.Categories.Create;

public class CreateCategoryHandler(
    ICategoryRepository categoryRepository, 
    IUnitOfWork unitOfWork,
    ICategoryQueries categoryQueries ) : IRequestHandler<CreateCategoryCommand, Result<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly ICategoryQueries _categoryQueries = categoryQueries;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {

        Result<Category> categoryResult = Category.Create(request.Name);
        if (categoryResult.IsFailure)
        {
            return Result<CategoryResponse>.Failure(categoryResult.Error);
        }

        Category newCategory = categoryResult.Value;

        await _categoryRepository.AddAsync(newCategory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return await _categoryQueries.GetResponseByIdAsync(newCategory.Id, cancellationToken);
    }
}