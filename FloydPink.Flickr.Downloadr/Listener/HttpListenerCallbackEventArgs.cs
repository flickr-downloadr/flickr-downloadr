using System;
using System.Collections.Specialized;

namespace FloydPink.Flickr.Downloadr.Listener
{
    public class HttpListenerCallbackEventArgs : EventArgs
    {
        public NameValueCollection QueryStrings { get; private set; }

        public HttpListenerCallbackEventArgs(NameValueCollection queryStrings)
        {
            this.QueryStrings = queryStrings;
        }
    }
}
