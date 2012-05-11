using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public interface IOAuthManager
    {
        string AccessToken { get; set; }
        
        event EventHandler<AuthenticatedEventArgs> Authenticated;

        string BeginAuthorization();
        HttpWebRequest PrepareAuthorizedRequest(IDictionary<string, string> parameters);
        dynamic MakeAuthenticatedRequest(string methodName, IDictionary<string, string> parameters = null);
    }
}
