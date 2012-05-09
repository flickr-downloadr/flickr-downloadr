using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloydPink.Flickr.Downloadr.Views
{
    public interface ILoginView : IBaseView
    {
        string UserName { get; set; }

        void ShowLoggedInControl();
        void ShowLoggedOutControl();
        void OpenAuthorizationUrl(string requestAuthUrl);
    }
}
