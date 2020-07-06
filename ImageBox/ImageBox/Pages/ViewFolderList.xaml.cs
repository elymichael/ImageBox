﻿namespace ImageBox.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewFolderList : ContentPage
    {
        public ViewFolderList()
        {
            InitializeComponent();

            BindingContext = this;

            LoadDirectory();
        }

        private readonly ObservableCollection<DestinationFolder> _destinationFolder = new ObservableCollection<DestinationFolder>();

        public event EventHandler<EventArgs> OperationCompleted;

        private bool _isPageOpen = false;
        private async void LoadDirectory()
        {
            try
            {
                ImageDestinationDisplay.ItemsSource = _destinationFolder;
                await Task.Run(async () =>
                {
                    _destinationFolder.Clear();

                    List<DestinationFolder> folders = await FileManager.GetFolders();
                    folders.Sort();

                    foreach (DestinationFolder df in folders)
                    {
                        df.Images = FileManager.GetSortedImages(df.Name);
                        _destinationFolder.Add(df);
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        badgeTitle.Text = folders.Count.ToString();
                    });
                });
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", "An error has ocurred: " + ex.Message, "OK");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var settingsPage = new SettingsPage();
            await Navigation.PushModalAsync(settingsPage);
        }

        public ICommand ClosePage => new Command(OnDismissButtonClicked);

        private async void OnDismissButtonClicked()
        {
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
        }

        public ICommand AddFolder => new Command(OnAddButtonClicked);

        private async void OnAddButtonClicked()
        {
            try
            {
                string result = await App.Current.MainPage.DisplayPromptAsync("New Folder", "Add your folder name");
                if (result != null)
                {                    
                    await Task.Run(() => {
                        FileManager.CreateFolder(result);
                        LoadDirectory(); 
                    });
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", "An error has ocurred: " + ex.Message, "OK");
            }
        }

        private async void ViewFolderPage_OperationCompleted(object sender, EventArgs e)
        {            
            (sender as ViewFolderPage).OperationCompleted -= ViewFolderPage_OperationCompleted;
            await Task.Run(() => {
                _isPageOpen = false;
                LoadDirectory(); 
            });
        }

        private async void ImageDestinationDisplay_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (e.Item == null) return;
                if (_isPageOpen) return;

                _isPageOpen = true;

                string folderName = ((DestinationFolder)e.Item).Name;

                var viewFolderPage = new ViewFolderPage(folderName);
                viewFolderPage.OperationCompleted += ViewFolderPage_OperationCompleted;
                await Navigation.PushModalAsync(viewFolderPage, true);
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", "An error has ocurred: " + ex.Message, "OK");
            }
        }        
    }
}