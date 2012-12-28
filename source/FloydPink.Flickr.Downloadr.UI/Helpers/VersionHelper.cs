using System.Reflection;

namespace FloydPink.Flickr.Downloadr.UI.Helpers
{
    public class VersionHelper
    {
        public static string GetVersionString()
        {
            return string.Format("beta (v{0})", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        } 
    }
}