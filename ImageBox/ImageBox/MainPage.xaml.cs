namespace ImageBox
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    using ImageBox.Pages;
    using System.IO;

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            loadConfiguration();
        }

        private void loadConfiguration()
        {
            BindingContext = this;

            LoadBitmapCollection();

            loadDirectory();
        }

        ObservableCollection<UnsortedImage> _unsortedImages = new ObservableCollection<UnsortedImage>();
        ObservableCollection<UnsortedImage> UnsortedImages { get { return _unsortedImages; } }

        ObservableCollection<DestinationFolder> _destinationFolder = new ObservableCollection<DestinationFolder>();
        ObservableCollection<DestinationFolder> ImagesDestinationFolder { get { return _destinationFolder; } }

        private int pointer = 0;

        async void LoadBitmapCollection()
        {
            try
            {
                ImageList imageList = await ImageFolderScan.GetImages("https://raw.githubusercontent.com/xamarin/docs-archive/master/Images/stock/small/stock.json");

                // Create an Image object for each bitmap
                foreach (string filepath in imageList.Photos)
                {
                    _unsortedImages.Add(new UnsortedImage(filepath));
                }

                setImages();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An unexpected error was found: " + ex.Message, "Ok");
            }
            finally
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }            
        }

        public ICommand OpenTrash => new Command(OnOpenTrash);
        public ICommand RefreshPage => new Command(OnRefreshPage);

        private async void OnOpenTrash()
        {
            var trashPage = new TrashPage();
            await Navigation.PushModalAsync(trashPage);
        }

        private void OnRefreshPage()
        {            
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            _unsortedImages.Clear();

            LoadBitmapCollection();
        }

        void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    pointer++;
                    break;
                case SwipeDirection.Right:
                    pointer--;
                    break;
                case SwipeDirection.Up:
                    pointer++;
                    break;
                case SwipeDirection.Down:
                    // Handle the swipe
                    break;
            }
            setImages();
        }

        private void setImages()
        {
            txtPhotoTotal.Text = string.Format("{0} of {1}", (pointer + 1), _unsortedImages.Count);
            imgCurrent.Source = _unsortedImages[pointer].ImagePath;
        }

        private void loadDirectory()
        {
            List<DestinationFolder> folders = ImageFolderScan.GetFolders("");
            foreach(DestinationFolder df in folders) {
                _destinationFolder.Add(df);
            }            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var viewFolderList = new ViewFolderList();
            await Navigation.PushModalAsync(viewFolderList);
        }
    }
}
