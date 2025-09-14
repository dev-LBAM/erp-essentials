using ErpEssentials.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Application.Contracts.Catalogs.Categories;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Categories.UpdateName;

public class UpdateCategoryNameHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ICategoryQueries categoryQueries) : IRequestHandler<UpdateCategoryNameCommand, Result<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly ICategoryQueries _categoryQueries = categoryQueries;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CategoryResponse>> Handle(UpdateCategoryNameCommand request, CancellationToken cancellationToken)
    {
        Category? Category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (Category is null)
        {
            return Result<CategoryResponse>.Failure(CategoryErrors.NotFound);
        }

        Result<Category> updateCategoryResult = Category.UpdateName(request.NewName);
        if (updateCategoryResult.IsFailure)
        {
            return Result<CategoryResponse>.Failure(updateCategoryResult.Error);
        }

        Category newCategory = updateCategoryResult.Value;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return await _categoryQueries.GetResponseByIdAsync(newCategory.Id, cancellationToken);
    }
}