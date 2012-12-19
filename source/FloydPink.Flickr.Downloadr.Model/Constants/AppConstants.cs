namespace FloydPink.Flickr.Downloadr.Model.Constants
{
    public static class AppConstants
    {
        public const string FlickrDictionaryContentKey = "_content";

        public const string AuthenticatedMessage =
            "<html><body>You have been authenticated. Please return to Flickr Downloadr.<br />" +
            "You could close this window at anytime.</body></html>";

        public const string BuddyIconUrlFormat = "http://farm{0}.staticflickr.com/{1}/buddyicons/{2}.jpg";
        public const string DefaultBuddyIconUrl = "http://www.flickr.com/images/buddyicon.gif";

        public const string PhotoUrlFormat = "http://farm{0}.staticflickr.com/{1}/{2}_{3}_{4}.{5}";
        public const string OriginalPhotoUrlFormat = "http://farm{0}.staticflickr.com/{1}/{2}_{3}_o.{4}";
        
        public const string MoreThan1000PhotosWarningFormat = "There are a total of {0} photos to be downloaded! This WILL take quite a while. Are you sure?";
        public const string MoreThan500PhotosWarningFormat = "You are going to download {0} photos. It could take a while. Continue?";
        public const string MoreThan100PhotosWarningFormat = "Are you sure you want to download {0} photos?";

    }
}