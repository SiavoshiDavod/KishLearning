using Newtonsoft.Json;

namespace SurveyWeb
{
    public static class CloneUsingJsonConvertExtension
    {
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}