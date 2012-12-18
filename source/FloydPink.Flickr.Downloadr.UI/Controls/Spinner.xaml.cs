using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FloydPink.Flickr.Downloadr.UI.Controls
{
    /// <summary>
    ///     Interaction logic for Spinner.xaml
    /// </summary>
    public partial class Spinner : UserControl
    {
        public static readonly DependencyProperty CancelButtonProperty =
            DependencyProperty.Register("CancelButton", typeof(bool), typeof(Spinner),
                                        new PropertyMetadata(CancelButtonCallback));

        public static readonly DependencyProperty ProgressPercentProperty =
            DependencyProperty.Register("ProgressPercent", typeof(string), typeof(Spinner),
                                        new PropertyMetadata(ProgressPercentCallback));

        public static readonly RoutedEvent SpinnerCanceledEvent =
            EventManager.RegisterRoutedEvent("SpinnerCanceled", RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler), typeof(Spinner));

        public event RoutedEventHandler SpinnerCanceled
        {
            add { AddHandler(SpinnerCanceledEvent, value); }
            remove { RemoveHandler(SpinnerCanceledEvent, value); }
        }

        public bool CancelButton
        {
            get { return (bool)GetValue(CancelButtonProperty); }
            set { SetValue(CancelButtonProperty, value); }
        }

        public string ProgressPercent
        {
            get { return (string)GetValue(ProgressPercentProperty); }
            set { SetValue(ProgressPercentProperty, value); }
        }

        public Spinner()
        {
            InitializeComponent();
        }

        private static void CancelButtonCallback(DependencyObject instance, DependencyPropertyChangedEventArgs e)
        {
            var spinner = (Spinner)instance;
            spinner.CancelButtonInternal.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void ProgressPercentCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var spinner = (Spinner)d;
            spinner.Progress.Text = e.NewValue.ToString();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var adorner = (FrameworkElement)VisualTreeHelper.GetParent(this);
            adorner.Visibility = Visibility.Collapsed;
            RaiseEvent(new RoutedEventArgs() {RoutedEvent= SpinnerCanceledEvent});
        }
    }
}