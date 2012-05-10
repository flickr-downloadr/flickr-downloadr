using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FloydPink.Flickr.Downloadr.Extensions;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public class UserRepository : RepositoryBase, IRepository<User>
    {
        internal override string repoFileName
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
