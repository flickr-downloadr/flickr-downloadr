using System;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public class OAuthCallbackEventArgs : EventArgs
    {
        public string Token { get; private set; }
        public string Verifier { get; private set; }

        public OAuthCallbackEventArgs(string token, string verifier)
        {
            this.Token = token;
            this.Verifier = verifier;
        }
    }
}
