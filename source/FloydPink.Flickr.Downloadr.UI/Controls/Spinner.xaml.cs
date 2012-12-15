﻿using System.Windows;
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
            DependencyProperty.Register("CancelButton", typeof (bool), typeof (Spinner),
                                        new PropertyMetadata(CancelButtonCallback));

        public Spinner()
        {
            InitializeComponent();
        }

        public bool CancelButton
        {
            get { return (bool) GetValue(CancelButtonProperty); }
            set { SetValue(CancelButtonProperty, value); }
        }

        private static void CancelButtonCallback(DependencyObject instance, DependencyPropertyChangedEventArgs e)
        {
            var spinner = (Spinner) instance;
            spinner.CancelButtonInternal.Visibility = (bool) e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var adorner = (FrameworkElement) VisualTreeHelper.GetParent(this);
            adorner.Visibility = Visibility.Collapsed;
        }
    }
}