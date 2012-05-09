using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public interface IOAuthManager
    {
        string RequestUserAuthorization();
        string ProcessUserAuthorization(string verifier);
        HttpWebRequest PrepareAuthorizedRequest(IDictionary<string, string> parameters);
    }
}
