using SenakLearn.JqGrid.Common;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SenakLearn.JqGrid
{

    public static class LinqExtensions
    {
        /// <summary>Orders the sequence by specific column and direction.</summary>
        /// <param name="query">The query.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="ascending">if set to true [ascending].</param>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string sortColumns, string direction)
        {
            direction = (string.IsNullOrEmpty(direction) ? "asc" : direction);

            string methodName = string.Format("OrderBy{0}",
                direction.ToLower() == "asc" ? "" : "descending");

            ParameterExpression parameter = Expression.Parameter(query.ElementType, "p");

            MemberExpression memberAccess = null;
            foreach (var sortColumn in sortColumns.Split(','))
            {
                foreach (var property in sortColumn.Split('.'))
                {
                    if (!string.IsNullOrEmpty(property))
                        memberAccess = MemberExpression.Property
                           (memberAccess ?? (parameter as Expression), property);
                }
            }
            
            if (memberAccess == null) return query;

            LambdaExpression orderByLambda = Expression.Lambda(memberAccess, parameter);

            MethodCallExpression result = Expression.Call(
                      typeof(Queryable),
                      methodName,
                      new[] { query.ElementType, memberAccess.Type },
                      query.Expression,
                      Expression.Quote(orderByLambda));

            return query.Provider.CreateQuery<T>(result);
        }
        

        public static IQueryable<T> Where<T>(this IQueryable<T> query,
            string column, object value, WhereOperation operation)
        {
            if (value == null) return query;
            if (string.IsNullOrEmpty(column))
                return query;

            ParameterExpression parameter = Expression.Parameter(query.ElementType, "p");

            MemberExpression memberAccess = null;
            foreach (var property in column.Split('.'))
                memberAccess = MemberExpression.Property
                    (memberAccess ?? (parameter as Expression), property);


            Type typeIfNullable = Nullable.GetUnderlyingType(memberAccess.Type);
            Type memberAccessType = memberAccess.Type;

            if (typeIfNullable != null)
            {
                memberAccessType = typeIfNullable;

                //memberAccess = MemberExpression.Convert(memberAccess,
                //                            typeIfNullable);
                //Expression convertExpr = Expression.Convert(
                //                          memberAccess,
                //                          typeIfNullable
                //                      );

            }

            //change param value type
            //necessary to getting bool from string
            object filterObject = null;
            try
            {
                filterObject = Convert.ChangeType(value, memberAccessType);
            }
            catch
            {
                return query;
            }

            ConstantExpression filter = Expression.Constant
                (

                    filterObject
                );

            //switch operation
            Expression condition = null;
            LambdaExpression lambda = null;
            switch (operation)
            {
                //equal ==
                case WhereOperation.Equal:
                    condition = Expression.Equal(MemberExpression.Convert(memberAccess,
                                            memberAccessType), filter);
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                //not equal !=
                case WhereOperation.NotEqual:
                    condition = Expression.NotEqual(MemberExpression.Convert(memberAccess,
                                            memberAccessType), filter);
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                //string.Contains()
                case WhereOperation.Contains:
                    var toLower = Expression.Call(MemberExpression.Convert(memberAccess,
                                            memberAccessType),
                                  typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
                    
                    condition = Expression.Call(toLower,
                                typeof(string).GetMethod("Contains"),
                                Expression.Constant(value.ToString().ToLower()));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
            }


            MethodCallExpression result = Expression.Call(
                   typeof(Queryable), "Where",
                   new[] { query.ElementType },
                   query.Expression,
                   lambda);
            if (result.ToString().Contains("(System.Decimal)"))
            {
                int startindex = result.ToString().IndexOf("(System.Decimal)");
                result.ToString().Insert(startindex,"                ");
            }
            return query.Provider.CreateQuery<T>(result);
        }
    }
}
