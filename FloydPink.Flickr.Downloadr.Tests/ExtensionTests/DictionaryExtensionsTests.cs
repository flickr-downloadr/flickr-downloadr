using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FloydPink.Flickr.Downloadr.Extensions;
using NUnit.Framework;

namespace FloydPink.Flickr.Downloadr.Tests.ExtensionTests
{
    [TestFixture]
    public class DictionaryExtensionsTests
    {
        [Test]
        public void WillGetValueFromJsonDictionary()
        {
            var dictionary = new Dictionary<string, object>() {
                {"key1", new Dictionary<string,object>() {{"_content", "value1"}}},
                {"key2", new Dictionary<string,object>() {{"_content", "value2"}}}
            };
            Assert.AreEqual("value1", dictionary.GetValueFromDictionary("key1"));
            Assert.AreEqual("value2", dictionary.GetValueFromDictionary("key2"));
        }
    }
}
