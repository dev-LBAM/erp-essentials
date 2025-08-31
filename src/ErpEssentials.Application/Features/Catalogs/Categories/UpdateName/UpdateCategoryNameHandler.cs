using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.SharedKernel.Abstractions;
using ErpEssentials.SharedKernel.Extensions;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;

namespace ErpEssentials.Application.Features.Catalogs.Categories.UpdateName;

public class UpdateCategoryNameHandler(ICategoryRepository CategoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryNameCommand, Result>
{
    private readonly ICategoryRepository _CategoryRepository = CategoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateCategoryNameCommand request, CancellationToken cancellationToken)
    {
        Category? Category = await _CategoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (Category is null)
        {
            return Result.Failure(CategoryErrors.NotFound);
        }

        string standardizedName = request.NewName.ToTitleCaseStandard();

        bool isNameUnique = await _CategoryRepository.IsNameUniqueAsync(standardizedName, cancellationToken);
        if (!isNameUnique)
        {
            return Result.Failure(CategoryErrors.NameInUse);
        }

        Result updateResult = Category.UpdateName(request.NewName);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
