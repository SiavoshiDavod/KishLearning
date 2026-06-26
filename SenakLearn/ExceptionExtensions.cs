namespace SenakLearn
{
    public static class ExceptionExtensions
    {
        public static string GetStackTraceWithMessage(this System.Exception ex)
        {
            if (string.IsNullOrEmpty(ex?.Message?.Trim())) return ex?.StackTrace;

            string result = ex.Message + "\r\n";
            result += ex.StackTrace;

            if (null != ex.InnerException)
            {
                string innerResult = GetStackTraceWithMessage(ex.InnerException);
                result += "\r\ninner : " + innerResult;
            }

            return result;
        }
    }
}