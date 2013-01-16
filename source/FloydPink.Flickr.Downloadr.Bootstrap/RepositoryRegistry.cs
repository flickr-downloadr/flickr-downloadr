using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Repository;
using StructureMap.Configuration.DSL;

namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For<IRepository<Token>>()
                .EnrichAllWith(s => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(typeof(IRepository<Token>), s))
                .Use<TokenRepository>();
            For<IRepository<User>>()
                .EnrichAllWith(s => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(typeof(IRepository<User>), s))
                .Use<UserRepository>();
        }
    }
}