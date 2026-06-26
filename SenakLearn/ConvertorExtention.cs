using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SenakLearn
{
    public static class ConvertorExtention
    {
        /// <summary>
        /// تبدیل اعداد انگلیسی به اعداد فارسی
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToPersianNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            var persian = new[] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
            var english = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            if (english.Any(input.Contains))
            {
                for (var j = 0; j < persian.Length; j++)
                    input = input.Replace(j.ToString(), persian[j]);
            }
            return input;
        }
        public static string ToEnglishNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            var persian = new[] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
            var english = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            if (persian.Any(input.Contains))
            {
                for (var j = 0; j < english.Length; j++)
                    input = input.Replace(persian[j], english[j]);
            }
            return input;
        }
        public static string GeogianToPersianString(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            var result= string.Format("{0}/{1}/{2} {3}:{4}:{5}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date),pc.GetHour(date),pc.GetMinute(date),pc.GetSecond(date));
            return result;
        }
        public static string GeogianToPersianStringDateOnly(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            var result = string.Format("{0}/{1}/{2}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));
            return result;
        }
        public static DateTime? PersianStringDateToDatetime(this string date,bool dateOnly=false)
        {
            DateTime? result = null;
            PersianCalendar pc = new PersianCalendar();

            if (!string.IsNullOrEmpty(date))
            {
                if (dateOnly == false)
                {
                    var date_time = date.Split(' ');
                    var _date = date_time[0].Split('/');
                    var _time = date_time[1].Split(':');

                    DateTime dateTime = new DateTime(int.Parse(_date[0].ToEnglishNumber()), int.Parse(_date[1].ToEnglishNumber()), int.Parse(_date[2].ToEnglishNumber()), int.Parse(_time[0].ToEnglishNumber()), int.Parse(_time[1].ToEnglishNumber()), int.Parse(_time[2].ToEnglishNumber()), pc);
                    result = DateTime.Parse(dateTime.ToString(CultureInfo.CreateSpecificCulture("en-US")));
                }else
                {
                    var date_time = date.Split(' ');
                    var _date = date_time[0].Split('/');

                    DateTime dateTime = new DateTime(int.Parse(_date[0].ToEnglishNumber()), int.Parse(_date[1].ToEnglishNumber()), int.Parse(_date[2].ToEnglishNumber()) , pc);
                    result = DateTime.Parse(dateTime.ToString(CultureInfo.CreateSpecificCulture("en-US")));
                }

            }
            return result;
        }
        //تبدیل رشته با جداکننده کاما به آرایه رشته
        public static string[] ToArrayForMultiDropDown(this string input)
        {
            return ToListForQuery(input).ToArray();
        }
        //تبدیل رشته با جداکننده کاما به لیست رشته
        public static List<string> ToListForQuery(this string input)
        {
            var list = new List<string>();
            if (string.IsNullOrEmpty(input))
                return list;
            var str = input.Split(',');
            foreach (var item in str)
            {
                if (!string.IsNullOrEmpty(item))
                    list.Add(item);
            }
            return list;
        }
    }
}