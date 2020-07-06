namespace ImageBox
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    using ImageBox.Pages;
    using Xamarin.Forms.Xaml;
    using System.Threading.Tasks;
    using Xamarin.Forms.PlatformConfiguration;

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            BindingContext = this;
            foldersViewer.OnMoveFileClicked += MoveImage;
            LoadBitmapCollection();
        }
        public ObservableCollection<ImageInfo> _unsortedImages = new ObservableCollection<ImageInfo>();
        ObservableCollection<ImageInfo> UnsortedImages { get { return _unsortedImages; } }

        private int pointer = 0;

        async void LoadBitmapCollection()
        {
            try
            {
                UnsortedImages.Clear();
                if(pointer == -1) { pointer = 0; }

                ImageList imageList = await FileManager.GetUnsortedImages();

                // Create an Image object for each bitmap
                foreach (string filepath in imageList.Photos)
                {
                    UnsortedImages.Add(new ImageInfo(filepath));
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    badgeTrash.Text = FileManager.GetTrashImages().Photos.Count.ToString();
                });
                SetImages();
            }
            catch (UnauthorizedAccessException)
            {
                await DisplayAlert("Access Permissions", "Request access permission to storage.", "Ok");
                FileManager.CheckPermission();
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
            if (FileManager.GetTrashImages().Photos.Count == 0)
            {
                await DisplayAlert("Empty Trash", "Swipe a photo UP to put into trash first.", "Ok");
                return;
            }

            var trashPage = new TrashPage();
            trashPage.OperationCompleted += TrashPage_OperationCompleted;
            await Navigation.PushModalAsync(trashPage);
        }

        private void TrashPage_OperationCompleted(object sender, EventArgs e)
        {
            (sender as TrashPage).OperationCompleted -= TrashPage_OperationCompleted;
            OnRefreshPage();
        }

        public void OnRefreshPage()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            UnsortedImages.Clear();

            LoadBitmapCollection();
        }
        private async void SwipeGestureRecognizer_SwipedRight(object sender, SwipedEventArgs e)
        {
            try
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

                    SetImages();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error has ocurred: " + ex.Message, "OK");
            }
        }
        private async void SwipeGestureRecognizer_SwipedLeft(object sender, SwipedEventArgs e)
        {
            try
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
                    SetImages();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error has ocurred: " + ex.Message, "OK");
            }
        }
        private async void SwipeGestureRecognizer_SwipedUp(object sender, SwipedEventArgs e)
        {
            try
            {
                if (e.Direction == SwipeDirection.Up)
                {
                    FileManager.MoveFileToTrash(UnsortedImages[pointer].ImagePath);
                    UnsortedImages.RemoveAt(pointer);
                    if (pointer >= (UnsortedImages.Count - 1))
                    {
                        pointer--;
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        badgeTrash.Text = FileManager.GetTrashImages().Photos.Count.ToString();
                    });
                    SetImages();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error has ocurred: " + ex.Message, "OK");
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

        private void SetImages()
        {
            if (UnsortedImages.Count > pointer)
            {
                string imagePath = FileManager.GetCompressedImage(UnsortedImages[pointer].ImagePath, 250, 250);
                Device.BeginInvokeOnMainThread(() =>
                {
                    txtPhotoTotal.Text = string.Format("{0} of {1}", (pointer + 1), UnsortedImages.Count);
                    imgCurrent.Source = ImageSource.FromFile(imagePath);
                });
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var viewFolderList = new ViewFolderList();
            viewFolderList.OperationCompleted += ViewFolderList_OperationCompleted;
            await Navigation.PushModalAsync(viewFolderList);
        }

        private void ViewFolderList_OperationCompleted(object sender, EventArgs e)
        {
            (sender as ViewFolderList).OperationCompleted -= TrashPage_OperationCompleted;
            OnRefreshPage();
        }

        async void MoveImage(string folderName)
        {
            try
            {
                FileManager.MoveFile(folderName, UnsortedImages[pointer].ImagePath);
                UnsortedImages.RemoveAt(pointer);
                if (pointer >= (UnsortedImages.Count - 1))
                {
                    pointer--;
                }
                SetImages();
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", "An error has ocurred: " + ex.Message, "OK");
            }                
        }
    }
}
