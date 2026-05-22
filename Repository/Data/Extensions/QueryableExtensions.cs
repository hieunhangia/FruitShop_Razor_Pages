using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Repository.Constants;

namespace Repository.Data.Extensions;

public static class QueryableExtensions
{
    extension<T>(IQueryable<T> query)
    {
        public IQueryable<T> DynamicOrderBy(string sortColumn,
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

        public IQueryable<T> WhereContainsUnaccent<TProperty>(Expression<Func<T, TProperty>> propertySelector,
            string searchTerm) =>
            string.IsNullOrWhiteSpace(searchTerm) ? query : BuildExpression(query, propertySelector, $"%{searchTerm}%");

        public IQueryable<T> WhereEqualsUnaccent<TProperty>(Expression<Func<T, TProperty>> propertySelector,
            string exactTerm) =>
            string.IsNullOrWhiteSpace(exactTerm) ? query : BuildExpression(query, propertySelector, exactTerm);
    }

    private static readonly MethodInfo UnaccentMethod = typeof(QueryableExtensions)
        .GetMethod(nameof(Unaccent))!;

    private static readonly MethodInfo ILikeMethod =
        typeof(NpgsqlDbFunctionsExtensions).GetMethod(nameof(NpgsqlDbFunctionsExtensions.ILike),
            [typeof(DbFunctions), typeof(string), typeof(string)])!;

    private static IQueryable<T> BuildExpression<T, TProperty>(
        IQueryable<T> query,
        Expression<Func<T, TProperty>> propertySelector,
        string matchTerm)
    {
        var efFunctions = Expression.Constant(EF.Functions);
        var propertyExpression = propertySelector.Body;
        if (propertyExpression.Type != typeof(string))
        {
            var toStringMethod = propertyExpression.Type.GetMethod(nameof(ToString), Type.EmptyTypes);
            if (toStringMethod == null)
            {
                throw new InvalidOperationException(
                    $"Type {propertyExpression.Type.Name} does not have a ToString method.");
            }

            propertyExpression = Expression.Call(propertyExpression, toStringMethod);
        }

        var unaccentProperty = Expression.Call(null, UnaccentMethod, propertyExpression);
        Expression<Func<string>> termClosure = () => matchTerm;
        var unaccentTerm = Expression.Call(null, UnaccentMethod, termClosure.Body);
        var iLikeCall = Expression.Call(null, ILikeMethod, efFunctions, unaccentProperty, unaccentTerm);
        return query.Where(Expression.Lambda<Func<T, bool>>(iLikeCall, propertySelector.Parameters[0]));
    }

    public static string Unaccent(string text)
    {
        throw new NotSupportedException(
            "This method is for use with Entity Framework Core and should not be called directly.");
    }
}