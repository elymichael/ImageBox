namespace ImageBox
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;

    public partial class App : Application
    {
        public static ObservableCollection<ImageInfo> _unsortedImages = new ObservableCollection<ImageInfo>();
        public static ObservableCollection<ImageInfo> _trashImages = new ObservableCollection<ImageInfo>();
        public static List<DestinationFolder> _destinationImageFolders= new List<DestinationFolder>();

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=79662850-16c1-4561-8b3d-fa2410c34bff;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
