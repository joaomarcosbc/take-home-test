namespace Fundo.Applications.Application.Common;

/// <summary>
/// Represents a paginated result set for a query, including the items and pagination metadata.
/// </summary>
/// <typeparam name="T">The type of the items in the paginated result.</typeparam>
/// <param name="Items">The list of items for the current page.</param>
/// <param name="TotalCount">The total number of items across all pages.</param>
/// <param name="PageNumber">The current page number (1-based).</param>
/// <param name="PageSize">The number of items per page.</param>
public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
);


