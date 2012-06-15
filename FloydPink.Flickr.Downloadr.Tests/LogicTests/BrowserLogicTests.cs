using System.Threading;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Logic;
using FloydPink.Flickr.Downloadr.Model;
using NUnit.Framework;

namespace FloydPink.Flickr.Downloadr.Tests.LogicTests
{
    [TestFixture]
    public class BrowserLogicTests
    {
        private ILoginLogic _loginLogic;
        private IBrowserLogic _logic;
        private User _user;
        private bool _asynchronouslyLoggedIn;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            Bootstrapper.Load();
            _loginLogic = Bootstrapper.GetInstance<ILoginLogic>();
            _logic = Bootstrapper.GetInstance<IBrowserLogic>();
        }

        [SetUp]
        public void SetupTest()
        {
            // do everything to take us to the browser window...
            if (!_loginLogic.IsUserLoggedIn(ApplyLoggedInUser))
            {
                _loginLogic.Login(ApplyLoggedInUser);
            }
        }

        private void ApplyLoggedInUser(User user)
        {
            _user = user;
            _asynchronouslyLoggedIn = true;
        }

        [Test]
        public void ProofOfConceptFor_AsynchronousTesting()
        {
            WaitTillLoggedIn();
            Assert.IsNotNull(_user);
        }

        [Test]
        public void GetPublicPhotos_WillGetPublicPhotos()
        {
            WaitTillLoggedIn();
            var photos = _logic.GetPublicPhotos(_user);
            Assert.IsNotNull(photos);
        }

        private void WaitTillLoggedIn()
        {
            while (!_asynchronouslyLoggedIn)
            {
                Thread.Sleep(1000);
            }
        }
    }
}