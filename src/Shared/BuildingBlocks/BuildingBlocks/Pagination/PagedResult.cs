namespace BuildingBlocks.Pagination;

public class PagedResult<TEntity>
{
    private const int UpperPageSize = 1000;
    private const int DefaultPageSize = 15;
    private const int DefaultPageIndex = 1;

    private int PageIndex { get; }
    private int PageSize { get; }
    private long TotalRecord { get; }
    private List<TEntity> DataResponse { get; }
    
    public bool HasNextPage => PageIndex * PageSize < TotalRecord;
    public bool HasPreviousPage => PageIndex > 1;
    
    
    private PagedResult(int pageIndex, int pageSize, long totalRecord, List<TEntity> dataResponse)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalRecord = totalRecord;
        DataResponse = dataResponse;
    }


    public static PagedResult<TEntity> Create(int pageIndex, int pageSize, int totalRecord, List<TEntity> dataResponse)
    {
        pageIndex = pageIndex <= 0 ? DefaultPageIndex : pageIndex;
        pageSize = pageSize <= 0 ? DefaultPageSize : pageSize > UpperPageSize ? UpperPageSize : pageSize;

        return new(pageIndex, pageSize, totalRecord, dataResponse);
    }

}