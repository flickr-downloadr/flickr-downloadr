using System.IO;
using System.Net;

// Thank you, Jeroen van Langen - http://stackoverflow.com/a/5175424/218882

namespace FloydPink.Flickr.Downloadr.UI.Cache
{
    public class HttpHelper
    {
        public static byte[] Get(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            return response.ReadToEnd();
        }

        public static void GetAndSaveToFile(string url, string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = Get(url);
                stream.Write(data, 0, data.Length);
            }
        }
    }
}