using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloydPink.Flickr.Downloadr.Listener
{
    public interface IHttpListenerManager
    {
        string ListenerAddress { get; }
        string ResponseString { get; set; }
        IAsyncResult SetupCallback();
        event EventHandler<HttpListenerCallbackEventArgs> RequestReceived;
    }
}
