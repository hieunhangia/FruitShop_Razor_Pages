using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Repository.Constants;

namespace Repository.Data.Extensions;

public static class QueryableExtensions
{
    private static readonly MethodInfo UnaccentMethod =
        typeof(QueryableExtensions).GetMethod(nameof(Unaccent)) ??
        throw new InvalidOperationException("Unaccent method not found");

    private static readonly MethodInfo ILikeMethod =
        typeof(NpgsqlDbFunctionsExtensions).GetMethod(nameof(NpgsqlDbFunctionsExtensions.ILike),
            [typeof(DbFunctions), typeof(string), typeof(string)])
        ?? throw new InvalidOperationException("ILike method not found");

    public static string Unaccent(string _) => throw new NotSupportedException(
        "This method is for use with Entity Framework Core and should not be called directly.");

    extension<T>(IQueryable<T> query)
    {
        public IOrderedQueryable<T> DynamicOrderBy(string sortColumn, SortDirection sortDirection,
            params object[] parameters)
        {
            var sortDirectionString = sortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var orderString = $"({sortColumn}) {sortDirectionString}";
            try
            {
                return query.OrderBy(orderString, parameters);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Invalid sort column {sortColumn}", e);
            }
        }

        public IQueryable<T> ApplyPaging(int pageIndex, int pageSize) =>
            query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

        public IQueryable<T> WhereContainsUnaccent<TProperty>(string searchTerm,
            Expression<Func<T, TProperty>> propertySelector)
            => query.ApplySingleUnaccentSearch(propertySelector, searchTerm, true);

        public IQueryable<T> WhereEqualsUnaccent<TProperty>(string searchTerm,
            Expression<Func<T, TProperty>> propertySelector)
            => query.ApplySingleUnaccentSearch(propertySelector, searchTerm, false);

        public IQueryable<T> WhereAnyContainsUnaccent(string searchTerm,
            params Expression<Func<T, object>>[] propertySelectors)
            => query.ApplyMultipleUnaccentSearch(searchTerm, true, true, propertySelectors);

        public IQueryable<T> WhereAnyEqualsUnaccent(string searchTerm,
            params Expression<Func<T, object>>[] propertySelectors) =>
            query.ApplyMultipleUnaccentSearch(searchTerm, false, true, propertySelectors);

        public IQueryable<T> WhereAllContainsUnaccent(string searchTerm,
            params Expression<Func<T, object>>[] propertySelectors) =>
            query.ApplyMultipleUnaccentSearch(searchTerm, true, false, propertySelectors);

        public IQueryable<T> WhereAllEqualsUnaccent(string searchTerm,
            params Expression<Func<T, object>>[] propertySelectors) =>
            query.ApplyMultipleUnaccentSearch(searchTerm, false, false, propertySelectors);

        private IQueryable<T> ApplySingleUnaccentSearch<TProperty>(Expression<Func<T, TProperty>> propertySelector,
            string searchTerm, bool isContains)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return query;
            }

            var matchTerm = isContains ? $"%{searchTerm}%" : searchTerm;
            var condition = BuildConditionExpression(propertySelector.Body, matchTerm);
            return query.Where(Expression.Lambda<Func<T, bool>>(condition, propertySelector.Parameters[0]));
        }

        private IQueryable<T> ApplyMultipleUnaccentSearch(string searchTerm, bool isContains, bool isAny,
            params Expression<Func<T, object>>[] propertySelectors)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || propertySelectors.Length == 0)
            {
                return query;
            }

            var matchTerm = isContains ? $"%{searchTerm}%" : searchTerm;
            var parameter = Expression.Parameter(typeof(T), "e");
            Expression? combinedExpression = null;

            foreach (var selector in propertySelectors)
            {
                var propertyExpression = selector.Body;
                if (propertyExpression is UnaryExpression { NodeType: ExpressionType.Convert } unary)
                {
                    propertyExpression = unary.Operand;
                }

                var visitor = new ParameterReplaceVisitor(selector.Parameters[0], parameter);
                propertyExpression = visitor.Visit(propertyExpression);

                var condition = BuildConditionExpression(propertyExpression, matchTerm);
                if (combinedExpression == null)
                {
                    combinedExpression = condition;
                }
                else
                {
                    combinedExpression = isAny
                        ? Expression.OrElse(combinedExpression, condition)
                        : Expression.AndAlso(combinedExpression, condition);
                }
            }

            return combinedExpression == null
                ? query
                : query.Where(Expression.Lambda<Func<T, bool>>(combinedExpression, parameter));
        }
    }


    private static MethodCallExpression BuildConditionExpression(Expression propertyExpression, string matchTerm)
    {
        if (propertyExpression.Type != typeof(string))
        {
            var toStringMethod = propertyExpression.Type.GetMethod(nameof(ToString), Type.EmptyTypes);
            if (toStringMethod == null)
            {
                throw new InvalidOperationException(
                    $"Type {propertyExpression.Type.FullName} does not have a ToString method.");
            }

            propertyExpression = Expression.Call(propertyExpression, toStringMethod);
        }

        var efFunctions = Expression.Constant(EF.Functions);
        var unaccentProperty = Expression.Call(null, UnaccentMethod, propertyExpression);

        Expression<Func<string>> termClosure = () => matchTerm;
        var unaccentTerm = Expression.Call(null, UnaccentMethod, termClosure.Body);

        return Expression.Call(null, ILikeMethod, efFunctions, unaccentProperty, unaccentTerm);
    }

    private class ParameterReplaceVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node) =>
            node == oldParameter ? newParameter : base.VisitParameter(node);
    }
}