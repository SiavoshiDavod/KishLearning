using System.Collections.Generic;
using System.Linq;

namespace BaseModel
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
