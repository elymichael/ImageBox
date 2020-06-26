namespace ImageBox
{
    using System;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    public partial class App : Application
    {
        public static ObservableCollection<UnsortedImage> _unsortedImages = new ObservableCollection<UnsortedImage>();
        public static ObservableCollection<UnsortedImage> _trashImages = new ObservableCollection<UnsortedImage>();
        public static ObservableCollection<DestinationFolder> _destinationImageFolders= new ObservableCollection<DestinationFolder>();

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
