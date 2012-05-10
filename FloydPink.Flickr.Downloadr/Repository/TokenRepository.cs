using System.IO;
using FloydPink.Flickr.Downloadr.Extensions;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Cryptography;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public class TokenRepository : RepositoryBase, IRepository<Token>
    {
        internal override string repoFileName
        {
            get { return "tokens.repo"; }
        }

        public Token Get()
        {
            return base.Read().FromJson<Token>();
        }

        public void Save(Token token)
        {
            base.Write(token.ToJson());
        }
    }
}
