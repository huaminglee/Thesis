using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Enumerations;
using System.Linq.Expressions;
using Thesis.Common.ViewModels;

namespace Thesis.Common.Helpers
{
    public partial class Ax
    {
        internal static string GetFilterMethodName(FilterCondition filterCondition)
        {
            string methodName;

            switch (filterCondition)
            {
                case FilterCondition.StartsWith:
                    methodName = "StartsWith";
                    break;
                case FilterCondition.EndsWith:
                    methodName = "EndsWith";
                    break;
                case FilterCondition.Contains:
                    methodName = "Contains";
                    break;
                default:
                    methodName = "StartsWith";
                    break;
            }

            return methodName;
        }

        internal static void GetFilterExpression(FilterViewModel filter, Type type, ParameterExpression p, ref Expression predicateBody)
        {
            Expression exp = null, exp2 = null, left = null, right = null;

            var pr = type.GetProperty(filter.PropertyName);
            if (pr == null) return;
            var prType = pr.PropertyType;
            left = Expression.Property(p, pr);
            if (prType == typeof(string))
                left = Expression.Call(left, prType.GetMethod("ToLower", System.Type.EmptyTypes));
            right = Expression.Constant(GetExpressionValue(filter.StartValue, prType), prType);

            switch (filter.FilterCondition)
            {
                case FilterCondition.StartsWith:
                    exp = Expression.Call(left, Ax.GetFilterMethodName(FilterCondition.StartsWith), null, right);
                    break;
                case FilterCondition.EndsWith:
                    exp = Expression.Call(left, Ax.GetFilterMethodName(FilterCondition.EndsWith), null, right);
                    break;
                case FilterCondition.Contains:
                    exp = Expression.Call(left, Ax.GetFilterMethodName(FilterCondition.Contains), null, right);
                    break;
                case FilterCondition.Equals:
                    exp = Expression.Equal(left, right);
                    break;
                case FilterCondition.NotEqual:
                    exp = Expression.NotEqual(left, right);
                    break;
                case FilterCondition.Greater:
                    exp = Expression.GreaterThan(left, right);
                    break;
                case FilterCondition.Less:
                    exp = Expression.LessThan(left, right);
                    break;
                case FilterCondition.Between:
                    exp = Expression.GreaterThan(left, right);
                    exp2 = Expression.LessThan(left, Expression.Constant(GetExpressionValue(filter.EndValue, prType), prType));
                    break;
                default:
                    exp = Expression.Equal(left, right);
                    break;
            }

            if (predicateBody == null)
                predicateBody = exp2 == null ? exp : Expression.And(exp, exp2);
            else
            {
                if (filter.WhereCondition == PredicateCondition.Or)
                {
                    predicateBody = exp2 == null ?
                        Expression.Or(predicateBody, exp) :
                        Expression.Or(predicateBody, Expression.And(exp, exp2));
                }
                else
                {
                    predicateBody = exp2 == null ?
                        Expression.And(predicateBody, exp) :
                        Expression.And(predicateBody, Expression.And(exp, exp2));
                }
            }
        }

        private static object GetExpressionValue(string value, Type type)
        {
            if (type == typeof(string))
                return value.ToLower();
            else if (type == typeof(int) || type == typeof(int?))
                return Ax.ConvertIntValueWithDefault(value);
            else if (type == typeof(decimal) || type == typeof(decimal?))
                return Ax.ConvertDecimalValueWithDefault(value);
            else if (type == typeof(float) || type == typeof(float?))
                return Ax.ConvertFloatValueWithDefault(value);
            else if (type == typeof(bool) || type == typeof(bool?))
                return Ax.ConvertBoolean(value);
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
                return Ax.ConvertDateTimeValueWithDefault(value);
            return value.ToLower();
        }
    }
}
