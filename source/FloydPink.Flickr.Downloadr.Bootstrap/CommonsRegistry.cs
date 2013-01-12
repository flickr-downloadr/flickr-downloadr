using System.IO;
using System.Reflection;
using StructureMap.Configuration.DSL;
using log4net;
using log4net.Config;

namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public class CommonsRegistry : Registry
    {
        public CommonsRegistry()
        {
            For<ILog>().AlwaysUnique().TheDefault.Is.
                ConstructedBy(s =>
                {
                    if (s.ParentType == null)
                        return LogManager.GetLogger(s.BuildStack.Current.ConcreteType);
                    return LogManager.GetLogger(s.ParentType);
                });

            var applicationPath = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location);
            var configFile = new FileInfo(Path.Combine(applicationPath, "flickrdownloadr.log4net"));
            XmlConfigurator.ConfigureAndWatch(configFile);
        }
    }
}