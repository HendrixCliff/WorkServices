namespace WorkServices.Application.Common.Pagination;

public sealed class PaginationQuery
{
    private int _page = 1;
    private int _pageSize = 20;

    public int Page
    {
        get => _page;
        init => _page = value <= 0 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = Math.Min(
            value <= 0 ? 20 : value,
            100);
    }
}