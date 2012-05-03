using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FloydPink.Flickr.Downloadr.Services;

namespace FloydPink.Flickr.Downloadr.Tests.ServicesTests
{
    [TestFixture]
    public class FlickrServicesTests
    {

        [Test]
        public void WillBuildServiceUrlFromMethodName()
        {
            Assert.AreEqual("http://api.flickr.com/services/rest/?method=My.Method&api_key=33fe2dc1389339c4e9cd77e9a90ebabf&format=json&nojsoncallback=1",
                FlickrServices.BuildServiceUrl("My.Method", new Dictionary<string, string>()));
        }

        [Test]
        public void WillBuildServiceUrlFromMethodNameAndParameters()
        {
            Assert.AreEqual("http://api.flickr.com/services/rest/?method=My.Method&api_key=33fe2dc1389339c4e9cd77e9a90ebabf&format=json&nojsoncallback=1" +
                "&testKey1=testValue1&testKey2=testValue2",
                FlickrServices.BuildServiceUrl("My.Method",
                                                new Dictionary<string, string>() { 
                                                    {"testKey1", "testValue1"},
                                                    {"testKey2", "testValue2"}
                                                }));
        }

        [Test]
        public void WillCallFlickrTestEchoSuccessfully()
        {
            var serviceUrl = FlickrServices.BuildServiceUrl("flickr.test.echo", new Dictionary<string, string>());
            Assert.IsNotNull((new FlickrServices()).makeFlickrServiceCall(serviceUrl));
        }

        [Test]
        public void WillGetValueFromJsonDictionary()
        {
            var dictionary = new Dictionary<string, object>() {
                {"key1", new Dictionary<string,object>() {{"_content", "value1"}}},
                {"key2", new Dictionary<string,object>() {{"_content", "value2"}}}
            };
            Assert.AreEqual("value1", FlickrServices.GetValueFromDictionary(dictionary, "key1"));
            Assert.AreEqual("value2", FlickrServices.GetValueFromDictionary(dictionary, "key2"));
        }

        [Test]
        public void WillCallFlickrTestEchoWithDummyParameter()
        {
            var serviceUrl = FlickrServices.BuildServiceUrl("flickr.test.echo", new Dictionary<string, string>() { { "dummyParam", "dummyValue" } });
            var dictionary = (new FlickrServices()).makeFlickrServiceCall(serviceUrl);
            Assert.Contains("dummyParam", dictionary.Keys);
            Assert.AreEqual("dummyValue", FlickrServices.GetValueFromDictionary(dictionary,"dummyParam"));
        }
    }
}
