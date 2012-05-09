using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FloydPink.Flickr.Downloadr.Repository;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Tests.RepositoryTests
{
    [TestFixture]
    public class RepositoryTests
    {
        private AccessTokenRepository _repository;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _repository = new AccessTokenRepository();
        }

        [Test]
        public void WillNotGetTokenWhenNoFileExists()
        {
            var token = _repository.Get();
            Assert.IsEmpty(token.TokenString);
            Assert.IsEmpty(token.Secret);
        }

        [Test]
        public void WillSaveAndGetToken()
        {
            var token = getNewAccessToken();
            _repository.Save(token);
            token = _repository.Get();
            Assert.AreEqual("token", token.TokenString);
            Assert.AreEqual("secret", token.Secret);
            _repository.Delete();
        }

        [Test]
        public void WillDeleteAndNotGetToken()
        {
            var token = getNewAccessToken();
            _repository.Save(token);
            _repository.Delete();
            token = _repository.Get();
            Assert.IsEmpty(token.TokenString);
            Assert.IsEmpty(token.Secret);
        }

        private Token getNewAccessToken()
        {
            return new Token("token", "secret");
        }

    }
}
