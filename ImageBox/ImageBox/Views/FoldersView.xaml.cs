﻿namespace ImageBox.Views
{
    using System;
    using System.Collections.Generic;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoldersView : ContentView
    {
        public FoldersView()
        {
            InitializeComponent();

            LoadDirectory();
        }

        public static List<DestinationFolder> _destinationImageFolders = new List<DestinationFolder>();
        List<DestinationFolder> DestinationImageFolders
        {
            get => _destinationImageFolders;
            set => _destinationImageFolders = value;
        }

        public delegate void OnMoveFileDelegate(string folderName);

        public OnMoveFileDelegate OnMoveFileClicked { get; set; }

        private async void LoadDirectory()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    folderLayout.Children.Clear();

                    DestinationImageFolders = await FileManager.GetFolders();
                    DestinationImageFolders.Sort();
                    foreach (DestinationFolder df in DestinationImageFolders)
                    {
                        FolderStackLayout stackLayout = new FolderStackLayout();
                        stackLayout.AddLabels(df.Name, Color.White, 0xf063);

                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.NumberOfTapsRequired = 1;
                        tapGestureRecognizer.Tapped += (s, e) =>
                        {
                            string folderName = ((StackLayout)s).AutomationId;
                            if (OnMoveFileClicked != null)
                                OnMoveFileClicked(folderName);
                        };
                        stackLayout.GestureRecognizers.Add(tapGestureRecognizer);

                        folderLayout.Children.Add(stackLayout);
                    }
                    CreateNewButtom();
                });
            }
            catch (UnauthorizedAccessException)
            {
                if (App.Current.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert("Access Permissions", "Request access permission to storage.", "Ok");
                    FileManager.CheckPermission();
                }
            }
            catch (Exception ex)
            {
                if (App.Current.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "An unexpected error was found: " + ex.Message, "Ok");
                }
            }
        }

        private void CreateNewButtom()
        {
            FolderStackLayout stackLayout = new FolderStackLayout();
            stackLayout.AddLabels("New Folder", Color.Gray, 0xf0fe);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.NumberOfTapsRequired = 1;
            tapGestureRecognizer.Tapped += OnTapGestureRecognizerNewButtonTapped;
            stackLayout.GestureRecognizers.Add(tapGestureRecognizer);

            folderLayout.Children.Add(stackLayout);
        }

        async void OnTapGestureRecognizerNewButtonTapped(object sender, EventArgs args)
        {
            try
            {
                string result = await App.Current.MainPage.DisplayPromptAsync("New Folder", "Add your folder name");
                if (result != null)
                {
                    FileManager.CreateFolder(result);
                    LoadDirectory();

                    if (OnMoveFileClicked != null)
                        OnMoveFileClicked(result);
                }
            }
            catch (Exception ex)
            {
                if (App.Current.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "An unexpected error was found: " + ex.Message, "Ok");
                }
            }
        }
    }
}