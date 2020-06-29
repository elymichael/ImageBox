namespace ImageBox
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    using ImageBox.Pages;

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
            foldersViewer.OnMoveFileClicked += MoveImage;
            LoadBitmapCollection();
        }

        ObservableCollection<UnsortedImage> UnsortedImages { get { return App._unsortedImages; } }

        private int pointer = 0;

        async void LoadBitmapCollection()
        {
            try
            {

                ImageList imageList = CacheDataImages.GetImages("temp");

                if (imageList.Photos.Count <= 0)
                {
                    var current = Connectivity.NetworkAccess;

                    if (current == NetworkAccess.Internet)
                    {
                        imageList = await ImageFolderScan.GetImages("https://raw.githubusercontent.com/xamarin/docs-archive/master/Images/stock/small/stock.json");
                        foreach (string filepath in imageList.Photos)
                        {
                            await CacheDataImages.SaveUrlImage(filepath);
                        }
                    }
                }

                imageList = CacheDataImages.GetImages("temp");
                // Create an Image object for each bitmap
                foreach (string filepath in imageList.Photos)
                {
                    UnsortedImages.Add(new UnsortedImage(filepath));
                }

                badgeTrash.Text = CacheDataImages.GetImages("trash").Photos.Count.ToString();

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
            if (CacheDataImages.GetImages("trash").Photos.Count == 0)
            {
                await DisplayAlert("Empty Trash", "Swipe a photo UP to put into trash first.", "Ok");
                return;
            }

            var trashPage = new TrashPage();
            await Navigation.PushModalAsync(trashPage);
        }

        private void OnRefreshPage()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            UnsortedImages.Clear();

            LoadBitmapCollection();
        }
        void SwipeGestureRecognizer_SwipedRight(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Right)
            {
                if (pointer > 0)
                {
                    pointer--;
                }
                else
                {
                    pointer = (UnsortedImages.Count - 1);
                }

                setImages();
            }
        }

        void SwipeGestureRecognizer_SwipedLeft(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Left)
            {
                if (pointer < (UnsortedImages.Count - 1))
                {
                    pointer++;
                }
                else
                {
                    pointer = 0;
                }
                setImages();
            }
        }

        void SwipeGestureRecognizer_SwipedUp(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Up)
            {
                CacheDataImages.MoveFile("trash", UnsortedImages[pointer].ImagePath);
                UnsortedImages.RemoveAt(pointer);
                if (pointer >= (UnsortedImages.Count - 1))
                {
                    pointer--;
                }
                badgeTrash.Text = CacheDataImages.GetImages("trash").Photos.Count.ToString();
                setImages();
            }
        }
        void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    break;
                case SwipeDirection.Right:
                    break;
                case SwipeDirection.Up:
                    break;
                case SwipeDirection.Down:
                    break;
            }
        }

        private void setImages()
        {
            txtPhotoTotal.Text = string.Format("{0} of {1}", (pointer + 1), UnsortedImages.Count);
            imgCurrent.Source = UnsortedImages[pointer].ImagePath;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var viewFolderList = new ViewFolderList();
            await Navigation.PushModalAsync(viewFolderList);
        }

        void MoveImage(string folderName)
        {
            CacheDataImages.MoveFile(folderName, UnsortedImages[pointer].ImagePath);
            UnsortedImages.RemoveAt(pointer);
            if (pointer >= (UnsortedImages.Count - 1))
            {
                pointer--;
            }
            setImages();
        }
    }
}
