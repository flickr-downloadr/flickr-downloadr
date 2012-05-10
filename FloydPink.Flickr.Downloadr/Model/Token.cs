using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloydPink.Flickr.Downloadr.Model
{
    public class Token
    {
        public Token()
        {
            this.TokenString = string.Empty;
            this.Secret = string.Empty;
        }

        public Token(string token, string secret)
        {
            this.TokenString = token;
            this.Secret = secret;
        }

        public string TokenString { get; set; }
        public string Secret { get; set; }
    }
}
