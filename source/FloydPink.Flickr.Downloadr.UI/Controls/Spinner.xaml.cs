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
        public static readonly DependencyProperty CancellableProperty =
            DependencyProperty.Register("Cancellable", typeof (bool), typeof (Spinner),
                new PropertyMetadata(CancellableChanged));

        public static readonly DependencyProperty PercentDoneProperty =
            DependencyProperty.Register("PercentDone", typeof (string), typeof (Spinner),
                new PropertyMetadata(PercentDoneChanged));

        public static readonly DependencyProperty OperationTextProperty =
            DependencyProperty.Register("OperationText", typeof (string), typeof (Spinner),
                new PropertyMetadata(OperationTextChanged));

        public static readonly RoutedEvent SpinnerCanceledEvent =
            EventManager.RegisterRoutedEvent("SpinnerCanceled", RoutingStrategy.Bubble,
                typeof (RoutedEventHandler), typeof (Spinner));

        public Spinner()
        {
            InitializeComponent();
        }

        public bool Cancellable
        {
            get { return (bool) GetValue(CancellableProperty); }
            set { SetValue(CancellableProperty, value); }
        }

        public string PercentDone
        {
            get { return (string) GetValue(PercentDoneProperty); }
            set { SetValue(PercentDoneProperty, value); }
        }

        public string OperationText
        {
            get { return (string) GetValue(OperationTextProperty); }
            set { SetValue(OperationTextProperty, value); }
        }

        public event RoutedEventHandler SpinnerCanceled
        {
            add { AddHandler(SpinnerCanceledEvent, value); }
            remove { RemoveHandler(SpinnerCanceledEvent, value); }
        }

        private static void CancellableChanged(DependencyObject instance, DependencyPropertyChangedEventArgs e)
        {
            ((Spinner) instance).CancelButton.Visibility = (bool) e.NewValue
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private static void OperationTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var spinner = (Spinner) d;
            spinner.OperationTextControl.Text = e.NewValue.ToString();
            spinner.OperationTextControl.Visibility = string.IsNullOrWhiteSpace(e.NewValue.ToString())
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private static void PercentDoneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Spinner) d).PercentDoneControl.Text = e.NewValue.ToString();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var adorner = (FrameworkElement) VisualTreeHelper.GetParent(this);
            adorner.Visibility = Visibility.Collapsed;
            RaiseEvent(new RoutedEventArgs {RoutedEvent = SpinnerCanceledEvent});
        }
    }
}