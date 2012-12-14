using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using log4net;

namespace FloydPink.Flickr.Downloadr.UI.Behaviors
{
    // http://stackoverflow.com/a/4797565/218882
    public static class ScrollToTopBehavior
    {
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(ScrollToTopBehavior));
        public static readonly DependencyProperty ScrollToTopProperty =
            DependencyProperty.RegisterAttached
                (
                    "ScrollToTop",
                    typeof (bool),
                    typeof (ScrollToTopBehavior),
                    new UIPropertyMetadata(false, OnScrollToTopPropertyChanged)
                );

        public static bool GetScrollToTop(DependencyObject obj)
        {
            return (bool) obj.GetValue(ScrollToTopProperty);
        }

        public static void SetScrollToTop(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollToTopProperty, value);
        }

        private static void OnScrollToTopPropertyChanged(DependencyObject dpo,
                                                         DependencyPropertyChangedEventArgs e)
        {
            Log.Debug("Entering OnScrollToTopPropertyChanged Method.");

            var itemsControl = dpo as ItemsControl;
            if (itemsControl != null)
            {
                DependencyPropertyDescriptor dependencyPropertyDescriptor =
                    DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof (ItemsControl));
                if (dependencyPropertyDescriptor != null)
                {
                    if ((bool) e.NewValue)
                    {
                        dependencyPropertyDescriptor.AddValueChanged(itemsControl, ItemsSourceChanged);
                    }
                    else
                    {
                        dependencyPropertyDescriptor.RemoveValueChanged(itemsControl, ItemsSourceChanged);
                    }
                }
            }
            
            Log.Debug("Leaving OnScrollToTopPropertyChanged Method.");
        }

        private static void ItemsSourceChanged(object sender, EventArgs e)
        {
            Log.Debug("Entering ItemsSourceChanged Method.");

            var itemsControl = sender as ItemsControl;
            EventHandler eventHandler = null;
            eventHandler = delegate
                               {
                                   if (itemsControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                                   {
                                       var scrollViewer = GetVisualChild<ScrollViewer>(itemsControl);
                                       scrollViewer.ScrollToTop();
                                       itemsControl.ItemContainerGenerator.StatusChanged -= eventHandler;
                                   }
                               };
            itemsControl.ItemContainerGenerator.StatusChanged += eventHandler;
            
            Log.Debug("Leaving ItemsSourceChanged Method.");
        }

        private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            Log.Debug("Entering GetVisualChild Method.");

            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = (Visual) VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            
            Log.Debug("Leaving GetVisualChild Method.");
            
            return child;
        }
    }
}