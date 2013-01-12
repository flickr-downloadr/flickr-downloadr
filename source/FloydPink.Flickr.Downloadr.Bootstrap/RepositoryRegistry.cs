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
                .Use<TokenRepository>()
                .EnrichWith(
                    s => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(typeof (IRepository<Token>), s));
            For<IRepository<User>>()
                .Use<UserRepository>()
                .EnrichWith(
                    s => DynamicProxyHelper.CreateInterfaceProxyWithTargetInterface(typeof (IRepository<User>), s));
        }
    }
}