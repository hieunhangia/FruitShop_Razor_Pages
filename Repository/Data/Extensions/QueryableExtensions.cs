using System.Linq.Dynamic.Core;
using Repository.Constants;

namespace Repository.Data.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> DynamicOrderBy<T>(this IQueryable<T> query, string sortColumn,
        SortDirection sortDirection)
    {
        var sortDirectionString = sortDirection == SortDirection.Ascending ? "ASC" : "DESC";
        var orderString = $"{sortColumn} {sortDirectionString}";
        try
        {
            return query.OrderBy(orderString);
        }
        catch
        {
            return query;
        }
    }
}