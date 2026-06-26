using System;

namespace SurveyWeb
{
    public class HandledException : Exception
    {
        public readonly string TargetUrl;
        public HandledException(string message):base(message)
        {

        }
        public HandledException(string message,string targetUrl) : base(message)
        {
            this.TargetUrl = targetUrl;
        }
    }
}