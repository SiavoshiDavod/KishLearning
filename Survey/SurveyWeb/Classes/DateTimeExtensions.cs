using System;

namespace SurveyWeb
{
    public static class DateTimeExtensions
    {/// <summary>
     /// in catch return DateTime.MinValue
     /// </summary>
     /// <param name="date"></param>
     /// <returns>Gregorian Date</returns>
        public static DateTime? ToGregorianDate(this string date)
        {
            if (string.IsNullOrWhiteSpace(date)) return null;
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
                return DateTime.Now;
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
                return (new System.Globalization.PersianCalendar().GetYear(dt)
                        + separator +
                        new System.Globalization.PersianCalendar().GetMonth(dt)
                        + separator +
                        new System.Globalization.PersianCalendar().GetDayOfMonth(dt) +
                        " - " + date.Hour + ":" + date.Minute);
            }
            catch
            {
                return string.Empty;
            }

        }
        public static string DateTimeNowPersian(string separator = "/",bool time=false)
        {
            try
            {
                DateTime dt = DateTime.Now;
                return (new System.Globalization.PersianCalendar().GetYear(dt)
                        + separator +
                        new System.Globalization.PersianCalendar().GetMonth(dt)
                        + separator +
                        new System.Globalization.PersianCalendar().GetDayOfMonth(dt)
                        +"_"
                        +dt.Hour
                         + separator +
                         dt.Minute
                          +separator +
                          dt.Second);
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
}