using FloydPink.Flickr.Downloadr.Logic;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Repository;
using NUnit.Framework;
using Rhino.Mocks;

namespace FloydPink.Flickr.Downloadr.Tests.LogicTests
{
    [TestFixture]
    public class LoginLogicTests
    {
        private IRepository<Token> _tokenRepository;
        private IRepository<User> _userRepository;

        [TestFixtureSetUp]
        public void Setup()
        {
            _tokenRepository = MockRepository.GenerateStub<IRepository<Token>>();
            _userRepository = MockRepository.GenerateStub<IRepository<User>>();
        }

        [Test]
        public void WillCallDeleteOnBothRepositoriesOnLogout()
        {
            var logic = new LoginLogic(null, _tokenRepository, _userRepository);
            logic.Logout();

            _tokenRepository.AssertWasCalled((t) => t.Delete());
            _userRepository.AssertWasCalled((u) => u.Delete());
        }
    }
}
