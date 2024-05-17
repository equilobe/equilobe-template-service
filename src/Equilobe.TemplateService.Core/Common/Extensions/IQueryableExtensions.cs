using System;
using Equilobe.TemplateService.Core.Common.Api;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.TemplateService.Core.Common.Extensions;

public static class IQueryableExtensions
{
    public static async Task<IPage<T>> ToPageAsync<T>(this IQueryable<T> source, int count)
    {
        var elements = await source.ToListAsync();

        return new Page<T>(count, elements);
    }

    public static async Task<IPage<T>> ToPageAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();

        var elements = source
            .Skip(pageSize * pageIndex)
            .Take(pageSize);

        return new Page<T>(count, await elements.ToListAsync());
    }

    public static async Task<IPage<T>> ToPageAsync<T>(this IQueryable<T> source, IPageFilter filter)
    {
        return await source.ToPageAsync(filter, CancellationToken.None);
    }

    public static async Task<IPage<T>> ToPageAsync<T>(this IQueryable<T> source, IPageFilter filter, CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);

        var elements = source
            .Skip(filter.PageSize * filter.PageIndex)
            .Take(filter.PageSize);

        return new Page<T>(count, await elements.ToListAsync(cancellationToken));
    }

    public static IPage<T> ToPage<T>(this IQueryable<T> source, IPageFilter filter)
    {
        var count = source.Count();

        var elements = source
            .Skip(filter.PageSize * filter.PageIndex)
            .Take(filter.PageSize);

        return new Page<T>(count, elements.ToList());
    }

    public static IQueryable<T> OrderBy<T, TKey>(
        this IQueryable<T> source,
        Expression<Func<T, TKey>> keySelector,
        bool sortDescending)
    {
        if (sortDescending)
        {
            return source.OrderByDescending(keySelector);
        }

        return source.OrderBy(keySelector);
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, ISortFilter filter)
    {
        if (string.IsNullOrEmpty(filter.SortBy))
        {
            return source;
        }

        if (filter.SortDescending)
        {
            source = source.OrderByDescending(filter.SortBy);
        }
        else
        {
            source = source.OrderBy(filter.SortBy);
        }

        return source;
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderBy(ToLambda<T>(propertyName));
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderByDescending(ToLambda<T>(propertyName));
    }


    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}

