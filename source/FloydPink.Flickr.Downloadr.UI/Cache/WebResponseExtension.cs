using System.IO;
using System.Net;

// Thank you, Jeroen van Langen - http://stackoverflow.com/a/5175424/218882

namespace FloydPink.Flickr.Downloadr.UI.Cache
{
    public static class WebResponseExtension
    {
        public static byte[] ReadToEnd(this WebResponse webresponse)
        {
            Stream responseStream = webresponse.GetResponseStream();

            using (var memoryStream = new MemoryStream((int) webresponse.ContentLength))
            {
                responseStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}