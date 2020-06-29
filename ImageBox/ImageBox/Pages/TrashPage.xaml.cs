﻿namespace ImageBox.Pages
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
                trashImages.Clear();
                flexLayout.Children.Clear();
                ImageList imageList = CacheDataImages.GetImages("trash");

                foreach (string filepath in imageList.Photos)
                {
                    trashImages.Add(new UnsortedImage(filepath));
                }

                if (trashImages.Count > 0)
                {
                    foreach (UnsortedImage _image in trashImages)
                    {
                        AddImage(_image.ImagePath);
                    }
                }
                else
                {
                    addEmptyTrashMessage("Empty Trash...", 18);
                }
            }
            catch (Exception ex)
            {
                addEmptyTrashMessage(string.Format("Cannot access list of bitmap files: {0}", ex.Message), 12);
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

        public ICommand CloseTrash => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            await Navigation.PopModalAsync();
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            List<UnsortedImage> listImages = new List<UnsortedImage>();
            if (Selected >= 1)
            {
                listImages = trashImages.Where(x => x.Selected == true).ToList<UnsortedImage>();
            }
            else
            {
                listImages = trashImages.ToList<UnsortedImage>();
            }

            bool answer = await DeleteConfirmNotifications(listImages.Count);
            if (answer)
            {
                foreach (UnsortedImage _image in listImages)
                {
                    UnsortedImages.Add(_image);
                    CacheDataImages.DeleteFile(_image.ImagePath);
                }
            }

            LoadBitmapCollection();

            ShowToastMessage("Photos deleted permanently!");
        }

        private async void btnRecover_Clicked(object sender, EventArgs e)
        {
            List<UnsortedImage> listImages = new List<UnsortedImage>();
            if (Selected >= 1)
            {
                listImages = trashImages.Where(x => x.Selected == true).ToList<UnsortedImage>();
            }
            else
            {
                listImages = trashImages.ToList<UnsortedImage>();
            }

            bool answer = await RecoverConfirmNotifications(listImages.Count);
            if (answer)
            {
                foreach (UnsortedImage _image in listImages)
                {
                    CacheDataImages.MoveFile("temp", _image.ImagePath);
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

        private void addEmptyTrashMessage(string message, int size)
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

            flexLayout.Children.Add(stackLayout);
        }
    }
}