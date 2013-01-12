using System;
using Castle.DynamicProxy;
using log4net;

namespace FloydPink.Flickr.Downloadr.Bootstrap
{
    public class DynamicProxyHelper
    {
        public static object CreateInterfaceProxyWithTargetInterface(Type interfaceType, object concreteObject)
        {
            var proxyGenerator = new ProxyGenerator();
            var result = proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceType,
                concreteObject,
                new[] { new LogInterceptor(LogManager.GetLogger(concreteObject.GetType())) });

            return result;
        }
    }
}