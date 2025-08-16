using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Categories.Create;

public class CreateCategoryHandler(ICategoryRepository categoryRepository) : IRequestHandler<CreateCategoryCommand, Result<Category>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;


    public async Task<Result<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        string standardizedName = request.Name.ToTitleCaseStandard();

        bool isNameUnique = await _categoryRepository.IsNameUniqueAsync(standardizedName, cancellationToken);
        if (!isNameUnique)
        {
            return Result<Category>.Failure(CategoryErrors.NameInUse);
        }

        Result<Category> categoryResult = Category.Create(standardizedName);
        if (categoryResult.IsFailure)
        {
            return Result<Category>.Failure(categoryResult.Error);
        }

        Category newCategory = categoryResult.Value;
        await _categoryRepository.AddAsync(newCategory, cancellationToken);

        return Result<Category>.Success(newCategory);
    }
}