namespace ImageBox.Pages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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


        private int Selected { get; set; } = 0;

        private int ImageAdded { get; set; } = 0;

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


        async void LoadBitmapCollection()
        {
            setButtonBindings(Selected);

            try
            {
                ImageList imageList = await ImageFolderScan.GetImages("https://raw.githubusercontent.com/xamarin/docs-archive/master/Images/stock/small/stock.json");

                // Create an Image object for each bitmap
                foreach (string filepath in imageList.Photos)
                {
                    AddImage(filepath);
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
            ImageAdded++;
            Image image = new Image
            {
                Source = ImageSource.FromUri(new Uri(filepath)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 120,
                WidthRequest = 120,
                Margin = 5
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.NumberOfTapsRequired = 1;
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                if (image.BackgroundColor == Color.White)
                {
                    image.BackgroundColor = Color.Transparent;
                    Selected--;
                }
                else
                {
                    image.BackgroundColor = Color.White;
                    Selected++;
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

            }
            else
            {
                flexLayout.Children.Clear();
            }
        }

        private void btnRecover_Clicked(object sender, EventArgs e)
        {
            if(Selected >= 1)
            {
                
            }
            else
            {
                flexLayout.Children.Clear();
            }
        }
    }
}