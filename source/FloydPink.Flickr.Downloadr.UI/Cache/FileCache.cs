using System;
using System.IO;

// Thank you, Jeroen van Langen - http://stackoverflow.com/a/5175424/218882
using System.Reflection;

namespace FloydPink.Flickr.Downloadr.UI.Cache
{
    public class FileCache
    {
        public static string AppCacheDirectory { get; set; }

        static FileCache()
        {
            // default cache directory - can be changed if needed from App.xaml
            AppCacheDirectory = string.Format("{0}/{1}/Cache/",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Assembly.GetExecutingAssembly().GetName().Name);
        }

        public static string FromUrl(string url)
        {
            // Check to see if the cache directory has been created
            if (!Directory.Exists(AppCacheDirectory))
            {
                // create it
                Directory.CreateDirectory(AppCacheDirectory);
            }

            // Cast the string into a Uri so we can access the image name without regex
            var uri = new Uri(url);
            var localFile = string.Format("{0}{1}", AppCacheDirectory, uri.Segments[uri.Segments.Length - 1]);

            if (!File.Exists(localFile))
            {
                HttpHelper.GetAndSaveToFile(url, localFile);
            }

            // The full path of the image on the local computer
            return localFile;
        }
    }
}