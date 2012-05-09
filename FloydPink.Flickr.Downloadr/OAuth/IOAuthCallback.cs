using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public interface IOAuthCallbackManager
    {
        string ListenerAddress { get; }
        IAsyncResult SetupCallback();
        event EventHandler<OAuthCallbackEventArgs> OAuthCallbackEvent;
    }
}
