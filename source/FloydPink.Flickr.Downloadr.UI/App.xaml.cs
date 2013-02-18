using System;
using System.Windows;
using System.Windows.Threading;
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
            base.OnStartup(e);

            DispatcherUnhandledException += OnDispatcherUnhandledException;

            Bootstrapper.Initialize();
            Log.Info("Application Start.");
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs eventArgs)
        {
            Log.Fatal("Unhandled Exception.", eventArgs.Exception);
            eventArgs.Handled = true;
            MessageBox.Show("An unexpected error has occured. Please submit the log file.", "Oops!");
            Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Info("Application Exit.");
            base.OnExit(e);
        }
    }
}