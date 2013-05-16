using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Thesis.Common.Enumerations;
using Thesis.Common.Helpers;
using Thesis.Common.ViewModels;
//using System.Data.Objects;
//using System.Data.Objects.DataClasses;
//using System.Data.Metadata.Edm;

namespace System.Linq
{
    public static class DynamicLibrary
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property, string dir)
        {
            string methodName = string.IsNullOrEmpty(dir) || dir.ToUpper() != "DESC" ? "OrderBy" : "OrderByDescending";
            return ApplyOrder<T>(source, property, methodName);
        }
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property, string dir)
        {
            string methodName = string.IsNullOrEmpty(dir) || dir.ToUpper() != "DESC" ? "ThenBy" : "ThenByDescending";
            return ApplyOrder<T>(source, property, methodName);
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            Type type = typeof(T);
            ParameterExpression p = Expression.Parameter(type, "x");
            Expression exp = Expression.Property(p, property);

            Expression expression = System.Linq.Expressions.Expression.Call(
                     typeof(Queryable),
                     methodName,
                     new Type[] { type, exp.Type },
                     source.Expression,
                     Expression.Lambda(exp, new System.Linq.Expressions.ParameterExpression[] { p })
                 );
            return (IOrderedQueryable<T>)source.Provider.CreateQuery(expression);
        }
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int skip, int take)
        {
            return skip >= 0 && take > 0 ? source.Skip(skip).Take(take) : source;
        }
        public static IQueryable<TSource> PagedItems<TSource>(this IQueryable<TSource> source, string sort, string dir, int skip, int take)
        {
            return source.OrderBy(sort, dir).Page(skip, take);
        }
        public static List<TSource> ToList<TSource>(this IQueryable<TSource> source, int skip, int take)
        {
            return source.Page(skip, take).ToList();
        }
        public static List<SelectItemViewModel<TIdType>> ToList<TIdType>(this IQueryable<SelectItemViewModel<TIdType>> source, int skip, int take, out int totalCount)
        {
            totalCount = source.Count();
            if (totalCount == 0) return new List<SelectItemViewModel<TIdType>>();
            return source.OrderBy(p => p.Text).Page(skip, take).ToList();
        }
        public static List<SelectItemViewModel<TIdType>> ToList<TIdType>(this IQueryable<SelectItemViewModel<TIdType>> source, ComboBoxViewModel viewModel, out int totalCount)
        {
            return ToList<TIdType>(source, viewModel, out totalCount, FilterCondition.StartsWith);
        }
        public static List<SelectItemViewModel<TIdType>> ToList<TIdType>(this IQueryable<SelectItemViewModel<TIdType>> source, ComboBoxViewModel viewModel, out int totalCount, FilterCondition filterCondition)
        {
            source = source.Filter(a => a.Text, viewModel.Filter, filterCondition);
            totalCount = source.Count();
            if (totalCount == 0) return new List<SelectItemViewModel<TIdType>>();
            return source.OrderBy(p => p.Text).Page(viewModel.Start, viewModel.Limit).ToList();
        }
        public static List<SelectItemViewModel> ToList(this IQueryable<SelectItemViewModel> source, int skip, int take, out int totalCount)
        {
            totalCount = source.Count();
            if (totalCount == 0) return new List<SelectItemViewModel>();
            return source.OrderBy(p => p.Text).Page(skip, take).ToList();
        }
        public static List<SelectItemViewModel> ToList(this IQueryable<SelectItemViewModel> source, ComboBoxViewModel viewModel, out int totalCount)
        {
            return ToList(source, viewModel, out totalCount, FilterCondition.StartsWith);
        }
        public static List<SelectItemViewModel> ToList(this IQueryable<SelectItemViewModel> source, ComboBoxViewModel viewModel, out int totalCount, FilterCondition filterCondition)
        {
            source = source.Filter(a => a.Text, viewModel.Filter, filterCondition);
            totalCount = source.Count();
            if (totalCount == 0) return new List<SelectItemViewModel>();
            return source.OrderBy(p => p.Text).Page(viewModel.Start, viewModel.Limit).ToList();
        }
        public static List<TSource> ToList<TSource>(this IQueryable<TSource> source, ComboBoxViewModel viewModel, out int totalCount)
        {
            return ToList<TSource>(source, viewModel, out totalCount, FilterCondition.StartsWith);
        }
        public static List<TSource> ToList<TSource>(this IQueryable<TSource> source, ComboBoxViewModel viewModel, out int totalCount, FilterCondition filterCondition)
        {
            Type type = typeof(TSource);
            var pr = type.GetProperty(viewModel.DisplayField);
            ParameterExpression p = Expression.Parameter(type, "p");
            Expression<Func<TSource, string>> exp = Expression.Lambda<Func<TSource, string>>(Expression.Property(p, pr), p);

            source = source.Filter(exp, viewModel.Filter, filterCondition);
            totalCount = source.Count();
            if (totalCount == 0) return new List<TSource>();
            return source.OrderBy(exp).Page(viewModel.Start, viewModel.Limit).ToList();
        }
        public static List<TSource> ToList<TSource>(this IQueryable<TSource> source, string sort, string dir, int skip, int take)
        {
            return source.PagedItems(sort, dir, skip, take).ToList();
        }
        public static List<TSource> ToList<TSource>(this IQueryable<TSource> source, IDataViewModel viewModel, out int totalCount)
        {
            AddPredicate(ref source, viewModel.FilterBase);
            AddPredicate(ref source, viewModel.Filter);            
            totalCount = source.Count();
            if (totalCount == 0) return new List<TSource>();
            return source.PagedItems(viewModel.Sort, viewModel.Dir, viewModel.Start, viewModel.Limit).ToList();
        }
        public static List<TSource> ToList<TSource>(this IQueryable<TSource> source, string sort, string dir, int skip, int take, out int totalCount)
        {
            totalCount = source.Count();
            if (totalCount == 0) return new List<TSource>();
            return source.PagedItems(sort, dir, skip, take).ToList();
        }
        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, string>> keySelector, string filter)
        {
            return source.Filter(keySelector, filter, FilterCondition.StartsWith);
        }
        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, string>> keySelector, string filter, FilterCondition filterCondition)
        {
            if (string.IsNullOrEmpty(filter) || filter.Trim().Length == 0)
                return source;
            else
            {
                Expression exp = Expression.Call(keySelector.Body, "ToLower", null);
                Expression expFilter = Expression.Call(exp, Ax.GetFilterMethodName(filterCondition), null, Expression.Constant(filter.ToLower(), typeof(string)));
                return source.Where(Expression.Lambda<Func<TSource, bool>>(expFilter, keySelector.Parameters));
            }
        }
        private static void AddPredicate<TSource>(ref IQueryable<TSource> source, List<FilterViewModel> viewModel)
        {
            if (viewModel != null && viewModel.Count > 0)
            {
                Type type = typeof(TSource);
                ParameterExpression p = Expression.Parameter(type, "p");
                Expression predicateBody = null;

                foreach (FilterViewModel filter in viewModel)
                    Ax.GetFilterExpression(filter, type, p, ref predicateBody);

                if (predicateBody != null)
                {
                    MethodCallExpression whereCallExpression = Expression.Call(
                        typeof(Queryable),
                        "Where",
                        new Type[] { source.ElementType },
                        source.Expression,
                        Expression.Lambda<Func<TSource, bool>>(predicateBody, new ParameterExpression[] { p }));

                    source = source.Provider.CreateQuery<TSource>(whereCallExpression);
                }
            }
        }
    }
}
