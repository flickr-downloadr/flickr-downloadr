using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public interface ITokenRepository
    {
        Token Get();
        void Save(Token token);
        void Delete();
    }
}
