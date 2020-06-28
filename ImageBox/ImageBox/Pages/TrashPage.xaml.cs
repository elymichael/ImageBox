namespace ImageBox.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [DesignTimeVisible(false)]
    public partial class TrashPage : ContentPage
    {
        public TrashPage()
        {
            InitializeComponent();

            BindingContext = this;

            LoadBitmapCollection();
        }


        ObservableCollection<UnsortedImage> UnsortedImages { get { return App._unsortedImages; } }
        ObservableCollection<UnsortedImage> trashImages { get { return App._trashImages; } }

        private int Selected { get; set; } = 0;

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

        void LoadBitmapCollection()
        {
            setButtonBindings(Selected);

            try
            {
                trashImages.Clear();
                flexLayout.Children.Clear();
                ImageList imageList = CacheDataImages.GetImages("trash");

                foreach (string filepath in imageList.Photos)
                {
                    trashImages.Add(new UnsortedImage(filepath));
                }

                foreach (UnsortedImage _image in trashImages)
                {
                    AddImage(_image.ImagePath);
                }
            }
            catch (Exception ex)
            {
                flexLayout.Children.Add(new Label
                {
                    Text = string.Format("Cannot access list of bitmap files: {0}", ex.Message)
                });
            }
            finally
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
        }

        private void AddImage(string filepath)
        {            
            Image image = new Image
            {
                Source = ImageSource.FromFile(filepath),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 120,
                WidthRequest = 120,
                Margin = 5,
                AutomationId = filepath

            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.NumberOfTapsRequired = 1;
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                UnsortedImage ui = trashImages.Where(x => x.ImagePath == image.AutomationId).First();

                if (image.BackgroundColor == Color.White)
                {
                    image.BackgroundColor = Color.Transparent;
                    Selected--;
                    ui.Selected = false;
                }
                else
                {
                    image.BackgroundColor = Color.White;
                    Selected++;
                    
                    ui.Selected = true;
                }
                setButtonBindings(Selected);
            };
            image.GestureRecognizers.Add(tapGestureRecognizer);

            flexLayout.Children.Add(image);
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ICommand CloseTrash => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            await Navigation.PopModalAsync();
        }

        private void btnDelete_Clicked(object sender, EventArgs e)
        {
            if (Selected >= 1)
            {
                var listImages = trashImages.Where(x => x.Selected == true).ToList<UnsortedImage>();
                foreach(UnsortedImage _image in listImages)
                {
                    UnsortedImages.Add(_image);
                    CacheDataImages.DeleteFile(_image.ImagePath);                
                }
            }
            else
            {                
                foreach(UnsortedImage _image in trashImages)
                {
                    UnsortedImages.Add(_image);
                    CacheDataImages.DeleteFile(_image.ImagePath);
                }
                              
            }
            this.LoadBitmapCollection();
        }

        private void btnRecover_Clicked(object sender, EventArgs e)
        {
            if(Selected >= 1)
            {
                var listImages = trashImages.Where(x => x.Selected == true).ToList<UnsortedImage>();
                foreach (UnsortedImage _image in listImages)
                {
                    CacheDataImages.MoveFile("temp", _image.ImagePath);
                }
            }
            else
            {
                foreach (UnsortedImage _image in trashImages)
                {
                    CacheDataImages.MoveFile("temp", _image.ImagePath);
                }
                
            }
            this.LoadBitmapCollection();
        }
    }
}