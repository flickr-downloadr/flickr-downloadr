using System.Windows;
using FloydPink.Flickr.Downloadr.Bootstrap;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrapper.Load();
        }
    }
}