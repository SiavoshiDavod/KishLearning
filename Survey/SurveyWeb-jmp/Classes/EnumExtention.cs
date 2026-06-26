using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SurveyWeb
{
    public static class EnumExtention
    {
        public static string GetDescription<TEnum>(TEnum value)
        {
            if (null == value) return string.Empty;
            var field = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();

        }


        public static Dictionary<string, int> GetEnumsProperty<T>() //where T : Enum
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var values = Enum.GetValues(typeof(T));

            foreach (object item in values)
            {
                System.Reflection.FieldInfo field = item.GetType().GetField(item.ToString());
                DescriptionAttribute attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    ?.OfType<DescriptionAttribute>()?.FirstOrDefault();
                try
                {
                    result.Add(attribute?.Description ?? item.ToString(), (int)item);
                }
                catch (Exception)
                {
                    result.Add(attribute?.Description ?? item.ToString(), (byte)item);
                }
                
            }

            return result;
        }



    }
}