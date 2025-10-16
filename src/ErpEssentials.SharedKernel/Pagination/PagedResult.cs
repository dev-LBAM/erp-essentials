namespace ErpEssentials.SharedKernel.Pagination;

public class PagedResult<T>(List<T> items, int totalItems, int page, int pageSize)
{
    public List<T> Items { get; } = items ?? [];
    public int TotalItems { get; } = totalItems;
    public int TotalPages { get; } = (int)Math.Ceiling(totalItems / (double)pageSize);
    public int Page { get; } = page;
    public int PageSize { get; } = pageSize;
}