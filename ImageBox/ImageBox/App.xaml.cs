namespace ImageBox
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

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
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
