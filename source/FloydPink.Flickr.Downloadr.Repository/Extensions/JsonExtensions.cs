using System.Web.Script.Serialization;
using log4net;

namespace FloydPink.Flickr.Downloadr.Repository.Extensions
{
    public static class JsonExtensions
    {
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(JsonExtensions));
        public static string ToJson(this object value)
        {
            Log.Debug("In ToJson Method.");

            return (new JavaScriptSerializer()).Serialize(value);
        }

        public static T FromJson<T>(this string json) where T : new()
        {
            Log.Debug("In FromJson Method.");

            if (string.IsNullOrEmpty(json))
                return new T();
            return (new JavaScriptSerializer()).Deserialize<T>(json);
        }
    }
}