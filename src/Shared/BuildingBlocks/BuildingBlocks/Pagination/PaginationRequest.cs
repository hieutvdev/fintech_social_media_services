namespace BuildingBlocks.Pagination;

public class PaginationRequest(int PageIndex = 1, int PageSize = 10)
{
    private const int UpperPageSize = 200;
    private const int DefaultPageSize = 20;
    private const int DefaultPageIndex = 0;

    public int PageIndex { get; } = PageIndex <= 0 ? DefaultPageIndex : PageIndex;

    public int PageSize { get; } =
        PageSize <= 0 ? DefaultPageSize : PageSize > UpperPageSize ? UpperPageSize : PageSize;
}
