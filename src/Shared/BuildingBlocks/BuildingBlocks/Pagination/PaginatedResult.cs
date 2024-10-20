namespace BuildingBlocks.Pagination;

public class PaginatedResult<TEntity>(int pageIndex, int pageSize, long totalRecord, IEnumerable<TEntity> dataResponse)
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long TotalRecord { get; } = totalRecord;
    public IEnumerable<TEntity> DataResponse { get; } = dataResponse;
}