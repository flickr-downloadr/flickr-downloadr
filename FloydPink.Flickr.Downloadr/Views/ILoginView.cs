using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Views
{
    public interface ILoginView : IBaseView
    {
        User User { get; set; }

        void ShowLoggedInControl();
        void ShowLoggedOutControl();
    }
}
