using System.Windows;
using FloydPink.Flickr.Downloadr.Bootstrap;
using log4net;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (App));

        protected override void OnStartup(StartupEventArgs e)
        {
            Log.Info("Starting Application.");
            base.OnStartup(e);
            Bootstrapper.Load();
        }
    }
}