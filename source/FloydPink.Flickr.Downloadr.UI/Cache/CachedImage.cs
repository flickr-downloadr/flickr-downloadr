using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

// Thank you, Jeroen van Langen - http://stackoverflow.com/a/5175424/218882 and Ivan Leonenko - http://stackoverflow.com/a/12638859/218882

namespace FloydPink.Flickr.Downloadr.UI.Cache
{
    public class CachedImage : Image
    {
        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register("ImageUrl",
            typeof (string), typeof (CachedImage), new PropertyMetadata("", ImageUrlPropertyChanged));

        static CachedImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (CachedImage),
                new FrameworkPropertyMetadata(typeof (CachedImage)));
        }

        public string ImageUrl
        {
            get { return (string) GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        private static void ImageUrlPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var url = (String) e.NewValue;

            if (String.IsNullOrEmpty(url))
                return;

            var cachedImage = (CachedImage) obj;
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(FileCache.FromUrl(url));
            bitmapImage.EndInit();
            cachedImage.Source = bitmapImage;
        }
    }
}