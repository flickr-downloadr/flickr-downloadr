using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Repository.Extensions;
using log4net;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public class UserRepository : RepositoryBase, IRepository<User>
    {
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserRepository));
        internal override string RepoFileName
        {
            get { return "user.repo"; }
        }

        #region IRepository<User> Members

        public User Get()
        {
            Log.Debug("In Get Method.");

            return base.Read().FromJson<User>();
        }

        public void Save(User value)
        {
            Log.Debug("Entering Save Method.");

            base.Write(value.ToJson());
            
            Log.Debug("Leaving Save Method.");
        }

        #endregion
    }
}