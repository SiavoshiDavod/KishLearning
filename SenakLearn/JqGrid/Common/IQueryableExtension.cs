using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SenakLearn.JqGrid.Common
{
    public static class IQueryableExtension
    {


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="gridHelper"></param>
        /// <returns></returns>
        public static IQueryable<T> FilterAndSortJqGrid<T>(this IQueryable<T> query, GridSettings gridSettings)
        {
            //filtring
            if (gridSettings.IsSearch && gridSettings.Where != null)
            {
                //And
                if (gridSettings.Where.groupOp == "AND")
                    foreach (var rule in gridSettings.Where.rules)

                        query = query.Where<T>(
                            rule.field, rule.data,
                            (global::SenakLearn.JqGrid.Common.WhereOperation)global::SenakLearn.JqGrid.Common.StringEnum.Parse(typeof(global::SenakLearn.JqGrid.Common.WhereOperation), rule.op));
                else
                {
                    //Or
                    var temp = (new List<T>()).AsQueryable();
                    if (gridSettings.Where != null && gridSettings.Where.rules != null)
                    {
                        foreach (var rule in gridSettings.Where.rules)
                        {
                            var t = query.Where<T>(
                            rule.field, rule.data,
                            (global::SenakLearn.JqGrid.Common.WhereOperation)global::SenakLearn.JqGrid.Common.StringEnum.Parse(typeof(global::SenakLearn.JqGrid.Common.WhereOperation), rule.op));
                            temp = temp.Concat<T>(t);
                        }
                    }
                    //remove repeating records
                    query = temp.Distinct<T>();
                }
            }

            //sorting
            if (!string.IsNullOrEmpty(gridSettings.SortColumn))
            {
                query = global::SenakLearn.JqGrid.Common.LinqExtensions.OrderBy<T>(query, (string) gridSettings.SortColumn,
                    (string) gridSettings.SortOrder);
            }
            return query;

        }


    }
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            var dd = Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);

            return dd;
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
