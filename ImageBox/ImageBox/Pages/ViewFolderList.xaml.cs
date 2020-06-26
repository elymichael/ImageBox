namespace ImageBox.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewFolderList : ContentPage
    {
        public ViewFolderList()
        {
            InitializeComponent();

            loadDirectory();
        }

        ObservableCollection<DestinationFolder> _destinationFolder = new ObservableCollection<DestinationFolder>();
        ObservableCollection<DestinationFolder> ImagesDestinationFolder { get { return _destinationFolder; } }

        private void loadDirectory()
        {
            ImageDestinationDisplay.ItemsSource = _destinationFolder;

            _destinationFolder.Add(new DestinationFolder { Name = "Clases", FolderType = FolderType.Images, Quantity = 20, });
            _destinationFolder.Add(new DestinationFolder { Name = "Familia", FolderType = FolderType.Images, Quantity = 5 });
            _destinationFolder.Add(new DestinationFolder { Name = "Música", FolderType = FolderType.Images, Quantity = 2 });
            _destinationFolder.Add(new DestinationFolder { Name = "Meditación", FolderType = FolderType.Images, Quantity = 12 });
            _destinationFolder.Add(new DestinationFolder { Name = "Investigación", FolderType = FolderType.Images, Quantity = 20 });
            _destinationFolder.Add(new DestinationFolder { Name = "Personal", FolderType = FolderType.Images, Quantity = 300 });
            _destinationFolder.Add(new DestinationFolder { Name = "Otros", FolderType = FolderType.Images, Quantity = 55 });
            _destinationFolder.Add(new DestinationFolder { Name = "Hijos", FolderType = FolderType.Images, Quantity = 450 });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var settingsPage = new SettingsPage();
            await Navigation.PushModalAsync(settingsPage);
        }
    }
}