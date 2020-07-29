using Newtonsoft.Json.Linq;

namespace Template.Common.Web.Infrastructure.Extensions
{
    public static class ResultEx
    {
        public static T ToObjectFromJsonString<T>(this string json)
        {
            var doc = JObject.Parse(json);
            var array = (JArray)doc["object"];

            return array.ToObject<T>();
        }
    }
}
