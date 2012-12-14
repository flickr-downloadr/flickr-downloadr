using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Repository.Extensions;
using log4net;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public class TokenRepository : RepositoryBase, IRepository<Token>
    {
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(TokenRepository));
        internal override string RepoFileName
        {
            get { return "token.repo"; }
        }

        #region IRepository<Token> Members

        public Token Get()
        {
            Log.Debug("In Get Method.");

            return base.Read().FromJson<Token>();
        }

        public void Save(Token value)
        {
            Log.Debug("Entering Save Method.");

            base.Write(value.ToJson());
            
            Log.Debug("Leaving Save Method.");
        }

        #endregion
    }
}