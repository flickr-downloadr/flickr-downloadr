using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Repository.Extensions;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public class UserRepository : RepositoryBase, IRepository<User>
    {
        internal override string RepoFileName
        {
            get { return "user.repo"; }
        }

        public User Get()
        {
            return base.Read().FromJson<User>();
        }

        public void Save(User value)
        {
            base.Write(value.ToJson());
        }
    }
}