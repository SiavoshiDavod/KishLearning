using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SurveyWeb.JqGrid.Common
{
    public static class Extension
    {
        public static TOutput ConvertToAnotherClass<T, TOutput>(this T source, TOutput target)
        {
            //find the list of properties in the source object
            Type sourceType = source.GetType();
            IList<PropertyInfo> sourcePropertyList =
              new List<PropertyInfo>(sourceType.GetProperties());
            //find the list of properties present in the target/destination 

            var targetType = target.GetType();
            IList<PropertyInfo> targetPropertyList =
               new List<PropertyInfo>(targetType.GetProperties());
            //assign value of source object property to the target object.

            foreach (PropertyInfo propertyTarget in targetPropertyList)
            {
                PropertyInfo property = null;
                //find the property which is present in the target object.

                property = sourcePropertyList.FirstOrDefault(m => m.Name == propertyTarget.Name);

                //if target property exists in the source
                if (property != null)
                {
                    // take value of source
                    object value = property.GetValue(source, null);
                    //assign it into the target property 
                    propertyTarget.SetValue(target, value, null);
                }
            }
            return target;
        }
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }
    }
}
