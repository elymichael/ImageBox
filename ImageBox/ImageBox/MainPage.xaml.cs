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
            foldersViewer.OnMoveFileClicked += MoveImage;
            LoadBitmapCollection();            
        }

        ObservableCollection<UnsortedImage> UnsortedImages { get { return App._unsortedImages; } }
        ObservableCollection<UnsortedImage> trashImages { get { return App._trashImages; } }

        private int pointer = 0;

        async void LoadBitmapCollection()
        {
            try
            {
                ImageList imageList = await ImageFolderScan.GetImages("https://raw.githubusercontent.com/xamarin/docs-archive/master/Images/stock/small/stock.json");

                // Create an Image object for each bitmap
                foreach (string filepath in imageList.Photos)
                {
                    UnsortedImages.Add(new UnsortedImage(filepath));
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

            UnsortedImages.Clear();

            LoadBitmapCollection();
        }

        void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    if(pointer < (UnsortedImages.Count - 1))
                    {
                        pointer++;
                    }
                    else
                    {
                        pointer = 0;
                    }
                    break;
                case SwipeDirection.Right:
                    if(pointer > 0)
                    {
                        pointer--;
                    }
                    else
                    {
                        pointer = (UnsortedImages.Count - 1);
                    }
                    break;
                case SwipeDirection.Up:
                    trashImages.Add(new UnsortedImage(UnsortedImages[pointer].ImagePath));
                    UnsortedImages.RemoveAt(pointer);                    
                    if (pointer >= (UnsortedImages.Count - 1))
                    {
                        pointer--;
                    }
                    break;
                case SwipeDirection.Down:
                    // Handle the swipe
                    break;
            }
            setImages();
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
            CacheDataImages.SaveImage(folderName);
            UnsortedImages.RemoveAt(pointer);
            if (pointer >= (UnsortedImages.Count - 1))
            {
                pointer--;
            }
            setImages();
        }
    }
}
