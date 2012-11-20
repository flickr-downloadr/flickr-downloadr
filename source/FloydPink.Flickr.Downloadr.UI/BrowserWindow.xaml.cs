using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using FloydPink.Flickr.Downloadr.Bootstrap;
using FloydPink.Flickr.Downloadr.Model;
using FloydPink.Flickr.Downloadr.Presentation;
using FloydPink.Flickr.Downloadr.Presentation.Views;

namespace FloydPink.Flickr.Downloadr.UI
{
    /// <summary>
    /// Interaction logic for BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window, IBrowserView
    {
        private readonly BrowserPresenter _presenter;
        private ObservableCollection<Photo> _photos;

        public User User { get; set; }

        public ObservableCollection<Photo> Photos
        {
            get { return _photos; }
            set
            {
                _photos = value;
                PhotoList.DataContext = this.Photos;
            }
        }

        public BrowserWindow(User user)
        {
            InitializeComponent();
            User = user;

            _presenter = Bootstrapper.GetPresenter<IBrowserView, BrowserPresenter>(this);
            _presenter.InitializeScreen();

        }

//        CollectionViewSource view = new CollectionViewSource();
//        ObservableCollection<Customer> Photos = new ObservableCollection<Customer>();
//        int currentPageIndex = 0;
//        int itemPerPage = 20;
//        int totalPage = 0;
//
//        private void ShowCurrentPageIndex()
//        {
//            this.tbCurrentPage.Text = (currentPageIndex + 1).ToString();
//        }
//        private void Window_Loaded(object sender, RoutedEventArgs e)
//        {
//            int itemcount = 107;
//            for (int j = 0; j < itemcount; j++)
//            {
//                Photos.Add(new Customer()
//                {
//                    ID = j,
//                    Name = "item" + j.ToString(),
//                    Age = 10 + j
//                });
//            }
//
//            // Calculate the total pages 
//            totalPage = itemcount / itemPerPage;
//            if (itemcount % itemPerPage != 0)
//            {
//                totalPage += 1;
//            }
//
//            view.Source = Photos;
//
//            view.Filter += new FilterEventHandler(view_Filter);
//
//            this.listView1.DataContext = view;
//            ShowCurrentPageIndex();
//            this.tbTotalPage.Text = totalPage.ToString();
//        }
//
//        void view_Filter(object sender, FilterEventArgs e)
//        {
//            int index = Photos.IndexOf((Customer)e.Item);
//
//            if (index >= itemPerPage * currentPageIndex && index < itemPerPage * (currentPageIndex + 1))
//            {
//                e.Accepted = true;
//            }
//            else
//            {
//                e.Accepted = false;
//            }
//        }
//
//        private void btnFirst_Click(object sender, RoutedEventArgs e)
//        {
//            // Display the first page 
//            if (currentPageIndex != 0)
//            {
//                currentPageIndex = 0;
//                view.View.Refresh();
//            }
//            ShowCurrentPageIndex();
//        }
//
//        private void btnPrev_Click(object sender, RoutedEventArgs e)
//        {
//            // Display previous page 
//            if (currentPageIndex > 0)
//            {
//                currentPageIndex--;
//                view.View.Refresh();
//            }
//            ShowCurrentPageIndex();
//        }
//
//        private void btnNext_Click(object sender, RoutedEventArgs e)
//        {
//            // Display next page 
//            if (currentPageIndex < totalPage - 1)
//            {
//                currentPageIndex++;
//                view.View.Refresh();
//            }
//            ShowCurrentPageIndex();
//        }
//
//        private void btnLast_Click(object sender, RoutedEventArgs e)
//        {
//            // Display the last page 
//            if (currentPageIndex != totalPage - 1)
//            {
//                currentPageIndex = totalPage - 1;
//                view.View.Refresh();
//            }
//            ShowCurrentPageIndex();
//        } 
    }
}
