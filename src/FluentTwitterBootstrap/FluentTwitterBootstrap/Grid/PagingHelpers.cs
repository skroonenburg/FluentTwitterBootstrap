using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentTwitterBootstrap.Grid
{
    public static class PagingHelpers
    {
        public static GridResult<T> Paged<T>(this IQueryable<T> queryable, PagingOption pagingOption)
        {
            return Paged(queryable, x => x, pagingOption);
        }

        private static Type GetSortPropertyType<T>(PagingOption pagingOption)
        {
            var currentType = typeof (T);

            foreach (var property in pagingOption.SortBy.Split('.'))
            {
                currentType = currentType.GetProperty(property, BindingFlags.Instance | BindingFlags.Public).PropertyType;
            }

            return currentType;
        }

        private static Expression<Func<T, TProp>> GetSortExpression<T, TProp>(PagingOption pagingOption, bool cast = false)
        {
            var param = Expression.Parameter(typeof(T), "x");

            Expression propertyExpression = param;
            foreach (var property in pagingOption.SortBy.Split('.'))
            {
                propertyExpression = Expression.Property(propertyExpression, property);
            }

            if (cast)
            {
                return Expression.Lambda<Func<T, TProp>>(Expression.Convert(propertyExpression, typeof(object)), param);
            }

            return Expression.Lambda<Func<T, TProp>>(propertyExpression, param);
        }
        
        private static IOrderedQueryable<T> SortQueryable<T, TProp>(IQueryable<T> queryable, PagingOption pagingOption)
        {
            var orderExpression = GetSortExpression<T, TProp>(pagingOption);
            return pagingOption.SortDirection == ListSortDirection.Ascending ? queryable.OrderBy(orderExpression) : queryable.OrderByDescending(orderExpression);
        }


        private static IOrderedQueryable<T> GetOrderedQueryable<T>(IQueryable<T> queryable, PagingOption pagingOption)
        {
            var propertyType = GetSortPropertyType<T>(pagingOption);
            var method = typeof(PagingHelpers).GetMethod("SortQueryable", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(new Type[] { typeof(T), propertyType });

            var orderedQueryable = (IOrderedQueryable<T>) method.Invoke(null, new object [] { queryable, pagingOption });

            return orderedQueryable;
        }

        private static IOrderedEnumerable<T> GetOrderedEnumerable<T>(IEnumerable<T> enumerable, PagingOption pagingOption)
        {
            var orderExpression = GetSortExpression<T, object>(pagingOption, true).Compile();

            return pagingOption.SortDirection == ListSortDirection.Ascending ? enumerable.OrderBy(orderExpression) : enumerable.OrderByDescending(orderExpression);
        }

        public static GridResult<TProjection> Paged<T, TProjection>(this IQueryable<T> queryable, Func<T, TProjection> projection, PagingOption pagingOption)
        {
            var totalCount = queryable.Count();

            if (!string.IsNullOrWhiteSpace(pagingOption.SortBy))
            {
                queryable = GetOrderedQueryable(queryable, pagingOption);
            }

            return new GridResult<TProjection>
                       {
                           Items = queryable.Skip(pagingOption.PageSize * (pagingOption.PageNumber - 1)).Take(pagingOption.PageSize).ToList().Select(projection).ToList(),
                           PageNumber = pagingOption.PageNumber,
                           PageSize = pagingOption.PageSize,
                           TotalCount = totalCount,
                           TotalPages = (int)Math.Ceiling(((double)totalCount) / pagingOption.PageSize)
                       };
        }


        public static GridResult<T> Paged<T>(this IEnumerable<T> enumerable, PagingOption pagingOption)
        {
            return Paged(enumerable, x => x, pagingOption);
        }

        public static GridResult<TProjection> Paged<T, TProjection>(this IEnumerable<T> enumerable, Func<T, TProjection> projection, PagingOption pagingOption)
        {
            var totalCount = enumerable.Count();

            if (!string.IsNullOrWhiteSpace(pagingOption.SortBy))
            {
                enumerable = GetOrderedEnumerable(enumerable, pagingOption);
            }

            return new GridResult<TProjection>
            {
                Items = enumerable.Skip(pagingOption.PageSize * (pagingOption.PageNumber - 1)).Take(pagingOption.PageSize).ToList().Select(projection).ToList(),
                PageNumber = pagingOption.PageNumber,
                PageSize = pagingOption.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(((double)totalCount) / pagingOption.PageSize)
            };
        }

        public static GridResult<T> AsGrid<T>(this IEnumerable<T> enumerable, PagingOption pagingOption)
        {
            var totalCount = enumerable.Count();

            return new GridResult<T>
            {
                Items = enumerable,
                PageNumber = pagingOption.PageNumber,
                PageSize = pagingOption.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(((double)totalCount) / pagingOption.PageSize)
            };
        }

        public static GridResult<TTransformed> Transform<TProjection, TTransformed>(this GridResult<TProjection> gridResult, Func<IEnumerable<TProjection>, IEnumerable<TTransformed>> transformationFunction)
        {
            return new GridResult<TTransformed>
            {
                Items = transformationFunction(gridResult.Items),
                PageNumber = gridResult.PageNumber,
                PageSize = gridResult.PageSize,
                TotalCount = gridResult.TotalCount,
                TotalPages = gridResult.TotalPages
            };
        }
    }
}