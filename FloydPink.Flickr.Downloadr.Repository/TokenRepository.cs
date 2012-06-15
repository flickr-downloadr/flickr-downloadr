using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Repository.Extensions;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public class TokenRepository : RepositoryBase, IRepository<Token>
    {
        internal override string RepoFileName
        {
            get { return "token.repo"; }
        }

        public Token Get()
        {
            return base.Read().FromJson<Token>();
        }

        public void Save(Token value)
        {
            base.Write(value.ToJson());
        }
    }
}