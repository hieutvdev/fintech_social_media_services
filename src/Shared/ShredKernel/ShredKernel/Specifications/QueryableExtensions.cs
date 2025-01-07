using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ShredKernel.Specifications;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereIfNotNullOrEmpty<T>(this IQueryable<T> source, string keySearch,
        Expression<Func<T, bool>> predicate)
    {
        if (string.IsNullOrEmpty(keySearch))
        {
            return source;
        }
        return source.Where(predicate);
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

    public static IQueryable<T> OrderByIf<T>(this IQueryable<T> source, bool condition,
        Expression<Func<T, bool>> keySelector, bool isDescending = false)
    {
        if (!condition)
        {
            return source;
        }
        return isDescending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
    }

    // public static IQueryable<T> IncludeIf<T>(this IQueryable<T> source, bool condition,
    //     Expression<Func<T, object>> navigationPropertyPath)
    // {
    //     return condition ? source.Inc
    // }

    public static IQueryable<TResult> SelectField<TSource, TResult>(this IQueryable<TSource> source,
        Expression<Func<TSource, TResult>> selector)
    {
        return source.Select(selector);
    }

    public static IQueryable<T> WhereIn<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keySelector,
        IEnumerable<TKey> values)
    {
        return source.Where(e => values.Contains(keySelector.Compile().Invoke(e)));
    }

    public static IQueryable<T> WhereBetween<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keySelector,
        TKey start, TKey end) where TKey : IComparable
    {
        return source.Where(e => keySelector.Compile().Invoke(e).CompareTo(start) >= 0 && keySelector.Compile().Invoke(e).CompareTo(end) <= 0);
    }

    public static Func<T, object> GetPropertyGetter<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var property = Expression.Property(parameter, propertyName);

        var conversion = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(conversion, parameter).Compile();
    }
    
    
    public static Expression<Func<T, object>> GetPropertyGetterV2<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var conversion = Expression.Convert(property, typeof(object)); // Convert the property to 'object'
        return Expression.Lambda<Func<T, object>>(conversion, parameter);
    }

    
    
 



}