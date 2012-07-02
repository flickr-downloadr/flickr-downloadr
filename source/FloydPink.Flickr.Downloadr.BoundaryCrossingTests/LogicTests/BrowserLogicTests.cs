using System.Collections.Generic;
using System.Threading;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Logic.Interfaces;
using FloydPink.Flickr.Downloadr.Model;
using NUnit.Framework;

namespace FloydPink.Flickr.Downloadr.BoundaryCrossingTests.LogicTests
{
    [TestFixture]
    public class BrowserLogicTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupTest()
        {
            // do everything to take us to the browser window...
            if (!_loginLogic.IsUserLoggedIn(ApplyLoggedInUser))
            {
                _loginLogic.Login(ApplyLoggedInUser);
            }
        }

        #endregion

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

        private void ApplyLoggedInUser(User user)
        {
            _user = user;
            _asynchronouslyLoggedIn = true;
        }

        private void WaitTillLoggedIn()
        {
            while (!_asynchronouslyLoggedIn)
            {
                Thread.Sleep(1000);
            }
        }

        [Test]
        public void GetPublicPhotos_WillGetPublicPhotos()
        {
            WaitTillLoggedIn();
            IEnumerable<Photo> photos = _logic.GetPublicPhotos(_user);
            Assert.IsNotNull(photos);
        }

        [Test]
        public void ProofOfConceptFor_AsynchronousTesting()
        {
            WaitTillLoggedIn();
            Assert.IsNotNull(_user);
        }
    }
}