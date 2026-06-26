using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SenakLearn
{
    public static class DateTimeExtensions
    {/// <summary>
     /// in catch return DateTime.MinValue
     /// </summary>
     /// <param name="date"></param>
     /// <returns>Gregorian Date</returns>
        public static DateTime ToGregorianDate(this string date)
        {
            if (string.IsNullOrWhiteSpace(date)) return DateTime.MinValue;
            date = date.Replace("-", "/");
            if (date == "") return DateTime.Now;
            try
            {
                string[] ymd = date.Split('/');
                int y = Convert.ToInt32(ymd[0]);
                int m = Convert.ToInt32(ymd[1]);
                int d = Convert.ToInt32(ymd[2]);
                if (ymd[2].Length == 4)
                {
                    y = Convert.ToInt32(ymd[2]);
                    d = Convert.ToInt32(ymd[0]);
                }

                return new System.Globalization.PersianCalendar().ToDateTime(y, m, d, 0, 0, 0, 0);
            }
            catch
            {
                return DateTime.MinValue;
                ;
            }
        }

        public static string ToPersianDate(this DateTime date, string separator = "/")
        {
            try
            {
                DateTime dt = new DateTime(date.Year, date.Month, date.Day);
                return (new System.Globalization.PersianCalendar().GetYear(dt)
                        + separator +
                        new System.Globalization.PersianCalendar().GetMonth(dt)
                        + separator +
                        new System.Globalization.PersianCalendar().GetDayOfMonth(dt));
            }
            catch
            {
                return string.Empty;
            }

        }
        public static string ToPersianDateTime(this DateTime date, string separator = "/")
        {
            try
            {
                DateTime dt = new DateTime(date.Year, date.Month, date.Day);
                var ret= (new System.Globalization.PersianCalendar().GetYear(dt)
                        + separator +
                        new System.Globalization.PersianCalendar().GetMonth(dt)
                        + separator +
                        new System.Globalization.PersianCalendar().GetDayOfMonth(dt) +
                        " - " + date.Hour + ":" + date.Minute);
                return ret;
            }
            catch
            {
                return string.Empty;
            }

        }
        public static string DateTimeNowPersian(string separator = "/")
        {
            try
            {
                DateTime dt = DateTime.Now;
                return (new System.Globalization.PersianCalendar().GetYear(dt)
                        + separator +
                        new System.Globalization.PersianCalendar().GetMonth(dt)
                        + separator +
                        new System.Globalization.PersianCalendar().GetDayOfMonth(dt));
            }
            catch
            {
                return string.Empty;
            }

        }
        public static int DateTimeNow()
        {
            try
            {
                DateTime dt = DateTime.Now;
                var month = new System.Globalization.PersianCalendar().GetMonth(dt).ToString();
                var day = new System.Globalization.PersianCalendar().GetDayOfMonth(dt).ToString();
                return int.Parse (new System.Globalization.PersianCalendar().GetYear(dt)
                        + (month.Length==2? month:("0"+ month)) + (day.Length == 2 ? day : ("0" + day))
                        );
            }
            catch
            {
                return 0;
            }
        }
    }
    public static class EnumExtention
    {
        public static string GetDescription<TEnum>(TEnum value)
        {
            if (null == value) return string.Empty;
            var field = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();

        }


        public static Dictionary<string, object> GetEnumsProperty<T>() //where T : Enum
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            var values = Enum.GetValues(typeof(T));

            foreach (object item in values)
            {
                System.Reflection.FieldInfo field = item.GetType().GetField(item.ToString());
                DescriptionAttribute attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    ?.OfType<DescriptionAttribute>()?.FirstOrDefault();

                result.Add(attribute?.Description ?? item.ToString(), (object)item);
            }

            return result;
        }



    }
}