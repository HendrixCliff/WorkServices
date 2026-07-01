namespace WorkServices.Application.Common.Pagination;

public sealed class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; init; }
        = Array.Empty<T>();

    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages =>
        (int)Math.Ceiling((double)TotalCount / PageSize);
}