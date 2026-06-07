namespace Application.Results;

public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalPage =>
    (int)Math.Ceiling(TotalCount / (double)PageSize); 
    public bool HasNextPage => Page < TotalPage;
    public bool HasPreviousPage => Page > 1;

    private PagedResult(IReadOnlyList<T> item, int totalcount, int page, int pageSize)
    {
        Items = item;
        TotalCount = totalcount;
        Page = page;
        PageSize = pageSize;
    }

    public static PagedResult<T> Ok(IReadOnlyList<T> items, int totalcount, int page, int pageSize)
            => new(items, totalcount, page, pageSize);
}