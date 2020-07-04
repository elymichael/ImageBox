namespace ImageBox.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Plugin.Toast;
    using FFImageLoading.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrashPage : ContentPage
    {
        public TrashPage()
        {
            InitializeComponent();

            BindingContext = this;

            LoadBitmapCollection();
        }        

        private int rowPosition = 0;
        private int colPosition = 0;

        public ObservableCollection<ImageInfo> _trashImages = new ObservableCollection<ImageInfo>();
        ObservableCollection<ImageInfo> TrashImages { get { return _trashImages; } }

        public event EventHandler<EventArgs> OperationCompleted;
        private int Selected { get; set; } = 0;

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _recoverText;
        public string RecoverText
        {
            get { return _recoverText; }
            set
            {
                if (_recoverText != value)
                {
                    _recoverText = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("RecoverText");
                }
            }
        }

        private string _deleteText;
        public string DeleteText
        {
            get { return _deleteText; }
            set
            {
                if (_deleteText != value)
                {
                    _deleteText = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("DeleteText");
                }
            }
        }
        #endregion

        void LoadBitmapCollection()
        {
            setButtonBindings(Selected);

            try
            {
                Selected = 0;
                TrashImages.Clear();
                flexLayout.Children.Clear();

                ImageList imageList = FileManager.GetTrashImages();

                foreach (string filepath in imageList.Photos)
                {
                    TrashImages.Add(new ImageInfo(filepath));
                }

                rowPosition = 0;
                colPosition = 0;
                if (TrashImages.Count > 0)
                {
                    foreach (ImageInfo _image in TrashImages)
                    {
                        AddImage(_image.ImagePath);
                    }
                }
                else
                {
                    AddEmptyTrashMessage("Empty Trash...", 18);
                }
            }
            catch (Exception ex)
            {
                AddEmptyTrashMessage(string.Format("Cannot access list of bitmap files: {0}", ex.Message), 12);
            }
            finally
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
        }

        private void AddImage(string filepath)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                CachedImage image = new CachedImage
                {
                    Source = ImageSource.FromFile(filepath),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 120,
                    WidthRequest = 120,
                    Aspect = Aspect.Fill,
                    AutomationId = filepath,
                    IsOpaque = true,
                    BackgroundColor = Color.Gray
                };
                
                image.DownsampleToViewSize = true;
                image.CacheDuration = new TimeSpan(5, 0, 0, 0);

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.NumberOfTapsRequired = 1;
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    ImageInfo ui = TrashImages.Where(x => x.ImagePath == image.AutomationId).First();

                    if (image.BackgroundColor == Color.White)
                    {
                        image.BackgroundColor = Color.Transparent;
                        image.Opacity = 1;
                        Selected--;
                        ui.Selected = false;
                    }
                    else
                    {
                        image.BackgroundColor = Color.White;
                        image.Opacity = 0.25;
                        Selected++;

                        ui.Selected = true;
                    }
                    setButtonBindings(Selected);
                };
                image.GestureRecognizers.Add(tapGestureRecognizer);

                Grid.SetColumn(image, colPosition);
                Grid.SetRow(image, rowPosition);

                flexLayout.Children.Add(image);

                colPosition++;
                if (colPosition == 3)
                {
                    colPosition = 0;
                    rowPosition++;
                }
            });
        }

        private void setButtonBindings(int selected)
        {
            if (selected >= 1)
            {
                DeleteText = string.Format("DELETE {0} PHOTOS", selected);
                RecoverText = string.Format("RECOVER {0} PHOTOS", selected);
            }
            else
            {
                DeleteText = "DELETE ALL PHOTOS";
                RecoverText = "RECOVER ALL PHOTOS";
            }
            btnDelete.Text = DeleteText;
            btnRecover.Text = RecoverText;
        }

        public ICommand CloseTrash => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            List<ImageInfo> listImages = new List<ImageInfo>();
            if (Selected >= 1)
            {
                listImages = TrashImages.Where(x => x.Selected == true).ToList<ImageInfo>();
            }
            else
            {
                listImages = TrashImages.ToList<ImageInfo>();
            }

            bool answer = await DeleteConfirmNotifications(listImages.Count);
            if (answer)
            {
                foreach (ImageInfo _image in listImages)
                {                    
                    FileManager.DeleteFile(_image.ImagePath);
                }
            }

            LoadBitmapCollection();

            ShowToastMessage("Photos deleted permanently!");
        }

        private async void btnRecover_Clicked(object sender, EventArgs e)
        {
            List<ImageInfo> listImages = new List<ImageInfo>();
            if (Selected >= 1)
            {
                listImages = TrashImages.Where(x => x.Selected == true).ToList<ImageInfo>();
            }
            else
            {
                listImages = TrashImages.ToList<ImageInfo>();
            }

            bool answer = await RecoverConfirmNotifications(listImages.Count);
            if (answer)
            {
                foreach (ImageInfo _image in listImages)
                {
                    FileManager.RestoreFile(_image.ImagePath);
                }
            }

            LoadBitmapCollection();

            ShowToastMessage("Photos moved to unsorted!");
        }

        private async Task<bool> DeleteConfirmNotifications(int total)
        {
            return await DisplayAlert(
                string.Format("Delete {0} photos permanently?", total),
                string.Format("{0} photos in trash will be deleted permanently from your device.", total),
                "Yes", "No");
        }

        private async Task<bool> RecoverConfirmNotifications(int total)
        {
            return await DisplayAlert(
                string.Format("Recover {0} photos?", total),
                string.Format("{0} photos in trash will be put back into unsorted.", total),
                "Yes", "No");
        }

        private void ShowToastMessage(string message)
        {
            CrossToastPopUp.Current.ShowToastMessage(message);
        }

        private void AddEmptyTrashMessage(string message, int size)
        {
            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(50, 0, 50, 0),
                Padding = new Thickness(50, 0, 50, 0)
            };

            stackLayout.Children.Add(new Label
            {
                Text = message,
                TextColor = Color.White,
                FontSize = size,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            });

            MainLayout.Children.Add(stackLayout);
        }        
    }
}