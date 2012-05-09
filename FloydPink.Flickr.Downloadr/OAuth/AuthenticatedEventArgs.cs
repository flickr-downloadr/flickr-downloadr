using System;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.OAuth
{
    public class AuthenticatedEventArgs : EventArgs
    {
        public User AuthenticatedUser { get; private set; }

        public AuthenticatedEventArgs(User user)
        {
            this.AuthenticatedUser = user;
        }
    }
}
