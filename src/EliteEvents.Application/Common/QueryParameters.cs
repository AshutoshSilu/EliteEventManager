namespace EliteEvents.Application.Common;

/// <summary>
/// Base query parameters for search, filter, pagination, and sorting.
/// </summary>
public class QueryParameters
{
    private int _pageSize = 10;
    private const int MaxPageSize = 100;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public string SortDirection { get; set; } = "asc";
    public string? FilterBy { get; set; }
    public string? FilterValue { get; set; }
}
