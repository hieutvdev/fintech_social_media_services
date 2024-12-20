namespace BuildingBlocks.Pagination;

public class PaginatedResult<TEntity>(int pageIndex, int pageSize, long totalRecord, IEnumerable<TEntity> dataResponse)
{
    private const int UpperPageSize = 200;
    private const int DefaultPageSize = 20;
    private const int DefaultPageIndex = 0;
    
    public int PageIndex { get; } = pageIndex <=0 ? DefaultPageIndex : pageIndex;
    public int PageSize { get; } = pageSize <= 0 ? DefaultPageSize : pageSize > UpperPageSize ? UpperPageSize : pageSize;
    public long TotalRecord { get; } = totalRecord;
    public IEnumerable<TEntity> DataResponse { get; } = dataResponse;
}