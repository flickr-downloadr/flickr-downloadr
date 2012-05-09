using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public interface IOAuthManager
    {
        string BeginAuthorization();
        event EventHandler<AuthenticatedEventArgs> Authenticated;
        HttpWebRequest PrepareAuthorizedRequest(IDictionary<string, string> parameters);
    }
}
