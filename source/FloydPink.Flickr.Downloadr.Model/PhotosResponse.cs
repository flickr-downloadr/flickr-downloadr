using System.Collections.Generic;

namespace FloydPink.Flickr.Downloadr.Model
{
    public class PhotosResponse
    {
        private readonly int _page;
        private readonly int _pages;
        private readonly int _perPage;
        private readonly int _total;
        private readonly IEnumerable<Photo> _photos;

        public PhotosResponse(int page, int pages, int perPage, int total, IEnumerable<Photo> photos)
        {
            _page = page;
            _pages = pages;
            _perPage = perPage;
            _total = total;
            _photos = photos;
        }

        public int Page
        {
            get { return _page; }
        }

        public int Pages
        {
            get { return _pages; }
        }

        public int PerPage
        {
            get { return _perPage; }
        }

        public int Total
        {
            get { return _total; }
        }

        public IEnumerable<Photo> Photos
        {
            get { return _photos; }
        }
    }
}