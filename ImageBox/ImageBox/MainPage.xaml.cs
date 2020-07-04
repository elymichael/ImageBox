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
                ImageList imageList = FileManager.GetUnsortedImages();

                // Create an Image object for each bitmap
                foreach (string filepath in imageList.Photos)
                {
                    UnsortedImages.Add(new ImageInfo(filepath));
                }

                badgeTrash.Text = FileManager.GetTrashImages().Photos.Count.ToString();

                SetImages();
            }
            catch (PermissionException)
            {
                await DisplayAlert("Access Permissions", "Request access permission to storage.", "Ok");
                CheckPermission();
            }
            catch (UnauthorizedAccessException)
            {
                await DisplayAlert("Access Permissions", "Request access permission to storage.", "Ok");
                CheckPermission();
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

                SetImages();
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
                SetImages();
            }
        }

        void SwipeGestureRecognizer_SwipedUp(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Up)
            {
                FileManager.MoveFileToTrash(UnsortedImages[pointer].ImagePath);
                UnsortedImages.RemoveAt(pointer);
                if (pointer >= (UnsortedImages.Count - 1))
                {
                    pointer--;
                }
                badgeTrash.Text = FileManager.GetTrashImages().Photos.Count.ToString();
                SetImages();
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
            txtPhotoTotal.Text = string.Format("{0} of {1}", (pointer + 1), UnsortedImages.Count);
            imgCurrent.Source = UnsortedImages[pointer].ImagePath;
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

        void MoveImage(string folderName)
        {
            FileManager.MoveFile(folderName, UnsortedImages[pointer].ImagePath);
            UnsortedImages.RemoveAt(pointer);
            if (pointer >= (UnsortedImages.Count - 1))
            {
                pointer--;
            }
            SetImages();
        }

        private void CheckPermission()
        {
            if (DeviceInfo.Platform.ToString() == Device.Android)
            {
                DependencyService.Get<ICheckFilePermission>().CheckPermission();
            }
        }
    }
}
